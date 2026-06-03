using ClauseAI.Application.Interfaces;
using ClauseAI.Application.Models;
using ClauseAI.Application.DTOs;
using ClauseAI.Domain.Entities;
using ClauseAI.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hangfire;

namespace ClauseAI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly ClauseAIDbContext _dbContext;
    private readonly IWebHostEnvironment _environment;
    private readonly IPdfTextExtractor _pdfTextExtractor;
    private readonly ITextChunker _textChunker;
    private readonly IEmbeddingService _embeddingService;
    private readonly IVectorSearchService _vectorSearchService;
    private readonly IRagService _ragService;
    private readonly IDocumentIngestionService _ingestionService;
    private readonly IChatCompletionService _chatCompletionService;

    public DocumentsController(
        ClauseAIDbContext dbContext,
        IWebHostEnvironment environment,
        IPdfTextExtractor pdfTextExtractor,
        ITextChunker textChunker,
        IEmbeddingService embeddingService,
        IVectorSearchService vectorSearchService,
        IRagService ragService,
        IDocumentIngestionService ingestionService,
        IChatCompletionService chatCompletionService)
    {
        _dbContext = dbContext;
        _environment = environment;
        _pdfTextExtractor = pdfTextExtractor;
        _textChunker = textChunker;
        _embeddingService = embeddingService;
        _vectorSearchService = vectorSearchService;
        _ragService = ragService;
        _ingestionService = ingestionService;
        _chatCompletionService = chatCompletionService;
    }

    [HttpPost("upload")]
    [RequestSizeLimit(50_000_000)]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is required.");
        }

        if (file.ContentType != "application/pdf")
        {
            return BadRequest("Only PDF files are allowed.");
        }

        var uploadsPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "..",
            "..",
            "storage",
            "uploads");

        Directory.CreateDirectory(uploadsPath);

        var storedFileName =
            $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        var fullPath = Path.Combine(uploadsPath, storedFileName);

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var document = new Document
        {
            Id = Guid.NewGuid(),
            FileName = file.FileName,
            StoredFileName = storedFileName,
            FilePath = fullPath,
            FileSize = file.Length,
            ContentType = file.ContentType
        };

        _dbContext.Documents.Add(document);

        await _dbContext.SaveChangesAsync();

        // await _ingestionService.ProcessAsync(document.Id);
        BackgroundJob.Enqueue<IDocumentIngestionService>(x => x.ProcessAsync(document.Id));

        return Ok(new
        {
            document.Id,
            document.FileName,
            document.Status,
            document.UploadedAtUtc
        });
    }

    [HttpPost("{documentId:guid}/extract")]
    public async Task<IActionResult> Extract(Guid documentId)
    {
        var document = await _dbContext.Documents.FindAsync(documentId);

        if (document == null)
        {
            return NotFound("Document not found.");
        }

        var extracted =
            await _pdfTextExtractor.ExtractAsync(
                document.Id,
                document.FilePath);

        return Ok(extracted);
    }

    [HttpPost("{documentId:guid}/chunk")]
    public async Task<IActionResult> Chunk(Guid documentId)
    {
        var document =
            await _dbContext.Documents.FindAsync(documentId);

        if (document == null)
        {
            return NotFound("Document not found.");
        }

        var extracted =
            await _pdfTextExtractor.ExtractAsync(
                document.Id,
                document.FilePath);

        var allChunks = new List<DocumentChunk>();

        foreach (var page in extracted.Pages)
        {
            var chunks = _textChunker.Chunk(
                page.Text,
                page.PageNumber);

            foreach (var chunk in chunks)
            {
                allChunks.Add(new DocumentChunk
                {
                    Id = Guid.NewGuid(),
                    DocumentId = document.Id,
                    PageNumber = chunk.PageNumber,
                    ChunkIndex = chunk.ChunkIndex,
                    Content = chunk.Content
                });
            }
        }

        _dbContext.DocumentChunks.AddRange(allChunks);

        await _dbContext.SaveChangesAsync();

        return Ok(new
        {
            documentId,
            chunkCount = allChunks.Count
        });
    }

    [HttpPost("{documentId:guid}/embed")]
    public async Task<IActionResult> Embed(Guid documentId)
    {
        var chunks = await _dbContext.DocumentChunks
            .Where(x => x.DocumentId == documentId)
            .ToListAsync();

        foreach (var chunk in chunks)
        {
            var embedding =
                await _embeddingService.GenerateAsync(chunk.Content);

            chunk.Embedding = new Pgvector.Vector(embedding);
        }

        await _dbContext.SaveChangesAsync();

        return Ok(new
        {
            documentId,
            embeddedChunks = chunks.Count
        });
    }

    [HttpPost("{documentId:guid}/search")]
    public async Task<IActionResult> Search(
        Guid documentId,
        [FromBody] SearchRequest request)
    {
        var results =
            await _vectorSearchService.SearchAsync(
                documentId,
                request.Question,
                request.TopK);

        return Ok(results);
    }

    [HttpPost("{documentId:guid}/ask")]
    public async Task<ActionResult<AskQuestionResponse>>
        Ask(Guid documentId, [FromBody] AskQuestionRequest request)
    {
        // Retrieve relevant chunks
        var chunks = await _vectorSearchService.SearchAsync(
                documentId,
                request.Question,
                request.TopK);

        // Build context
        var context = string.Join(
            "\n\n",
            chunks.Select(x =>
                $"[Page {x.PageNumber}]\n{x.Content}"));

        // Generate answer
        var answer = await _chatCompletionService.AskAsync(request.Question, context);

        // Create conversation
        var conversation = new Conversation
            {
                Id = Guid.NewGuid(),
                DocumentId = documentId,
                Title = request.Question.Length > 80
                        ? request.Question.Substring(0, 80)
                        : request.Question,
                CreatedAt = DateTime.UtcNow
            };

        _dbContext.Conversations.Add(conversation);

        // Save user message
        _dbContext.ConversationMessages.Add(
            new ConversationMessage
            {
                Id = Guid.NewGuid(),
                ConversationId = conversation.Id,
                Role = "user",
                Content = request.Question,
                CreatedAt = DateTime.UtcNow
            });

        // Save assistant message
        _dbContext.ConversationMessages.Add(
            new ConversationMessage
            {
                Id = Guid.NewGuid(),
                ConversationId = conversation.Id,
                Role = "assistant",
                Content = answer,
                CreatedAt = DateTime.UtcNow
            });

        await _dbContext.SaveChangesAsync();

        // Return response
        return Ok(new AskQuestionResponse
        {
            Answer = answer,

            Citations = chunks
                .Select(x => new CitationDto
                {
                    PageNumber = x.PageNumber,

                    Content =
                        x.Content.Length > 300
                            ? x.Content.Substring(0, 300) + "..."
                            : x.Content
                })
                .ToList()
        });
    }

    [HttpGet("{documentId:guid}/status")]
    public async Task<IActionResult> GetStatus(
        Guid documentId)
    {
        var document =
            await _dbContext.Documents
                .FirstOrDefaultAsync(x =>
                    x.Id == documentId);

        if (document == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            id = document.Id,
            status = document.Status.ToString()
        });
    }

    [HttpPost("{documentId:guid}/ask-stream")]
    public async Task AskStream(
        Guid documentId,
        [FromBody] AskQuestionRequest request)
    {
        Response.ContentType = "text/plain";

        var chunks =
            await _vectorSearchService.SearchAsync(
                documentId,
                request.Question,
                request.TopK);

        var context = string.Join(
            "\n\n",
            chunks.Select(x =>
                $"[Page {x.PageNumber}]\n{x.Content}"));

        await foreach (var token in _chatCompletionService.StreamAsync(
                request.Question,
                context))
        {
            await Response.WriteAsync(token);

            await Response.Body.FlushAsync();
        }
    }

    [HttpGet("{documentId:guid}/file")]
    public async Task<IActionResult> GetFile(Guid documentId)
    {
        var document = await _dbContext.Documents.FirstOrDefaultAsync(x => x.Id == documentId);

        if (document == null)
        {
            return NotFound();
        }

        var stream = System.IO.File.OpenRead(document.FilePath);

        return File(stream, "application/pdf");
    }
}