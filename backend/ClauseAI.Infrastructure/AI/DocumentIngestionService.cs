using ClauseAI.Application.Interfaces;
using ClauseAI.Domain.Entities;
using ClauseAI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using ClauseAI.Domain.Enums;

namespace ClauseAI.Infrastructure.AI;

public class DocumentIngestionService : IDocumentIngestionService
{
    private readonly ClauseAIDbContext _dbContext;
    private readonly IPdfTextExtractor _pdfTextExtractor;
    private readonly ITextChunker _textChunker;
    private readonly IEmbeddingService _embeddingService;

    public DocumentIngestionService(
        ClauseAIDbContext dbContext,
        IPdfTextExtractor pdfTextExtractor,
        ITextChunker textChunker,
        IEmbeddingService embeddingService)
    {
        _dbContext = dbContext;
        _pdfTextExtractor = pdfTextExtractor;
        _textChunker = textChunker;
        _embeddingService = embeddingService;
    }

    public async Task ProcessAsync(Guid documentId)
    {
        var document =
            await _dbContext.Documents
                .FirstOrDefaultAsync(x =>
                    x.Id == documentId);

        if (document == null)
        {
            throw new Exception("Document not found.");
        }

        try
        {
            
            document.Status = DocumentStatus.Processing;

            await _dbContext.SaveChangesAsync();

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
                    var embedding =
                        await _embeddingService.GenerateAsync(
                            chunk.Content);

                    allChunks.Add(new DocumentChunk
                    {
                        Id = Guid.NewGuid(),
                        DocumentId = document.Id,
                        PageNumber = chunk.PageNumber,
                        ChunkIndex = chunk.ChunkIndex,
                        Content = chunk.Content,
                        Embedding = new Vector(embedding)
                    });
                }
            }

            _dbContext.DocumentChunks.AddRange(allChunks);

            await _dbContext.SaveChangesAsync();

            document.Status = DocumentStatus.Ready;

            await _dbContext.SaveChangesAsync();
        }
        catch
        {
            document.Status = DocumentStatus.Failed;

            await _dbContext.SaveChangesAsync();

            throw;
        }
    }
}