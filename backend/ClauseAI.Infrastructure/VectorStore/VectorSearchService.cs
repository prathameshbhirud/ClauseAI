using ClauseAI.Application.Interfaces;
using ClauseAI.Application.Models;
using ClauseAI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace ClauseAI.Infrastructure.VectorStore;

public class VectorSearchService : IVectorSearchService
{
    private readonly ClauseAIDbContext _dbContext;
    private readonly IEmbeddingService _embeddingService;

    public VectorSearchService(
        ClauseAIDbContext dbContext,
        IEmbeddingService embeddingService)
    {
        _dbContext = dbContext;
        _embeddingService = embeddingService;
    }

    public async Task<List<SearchResult>> SearchAsync(
        Guid documentId,
        string question,
        int topK = 5)
    {
        var embedding =
            await _embeddingService.GenerateAsync(question);

        var vector = new Vector(embedding);

        var results = await _dbContext.DocumentChunks
            .Where(x =>
                x.DocumentId == documentId &&
                x.Embedding != null)
            .OrderBy(x =>
                x.Embedding!.CosineDistance(vector))
            .Take(topK)
            .Select(x => new SearchResult
            {
                ChunkId = x.Id,
                PageNumber = x.PageNumber,
                Content = x.Content,
                Similarity = 1 - x.Embedding!.CosineDistance(vector)
            })
            .ToListAsync();

        return results;
    }
}