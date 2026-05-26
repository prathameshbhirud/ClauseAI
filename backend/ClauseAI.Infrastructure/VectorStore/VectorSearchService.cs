using ClauseAI.Application.Interfaces;
using ClauseAI.Application.Models;
using ClauseAI.Domain.Entities;
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

        var normalizedQuestion =
            question.ToLower();

        // PURE SQL vector search
        var semanticResults =
            await _dbContext.DocumentChunks
                .Where(x =>
                    x.DocumentId == documentId &&
                    x.Embedding != null)
                .OrderBy(x =>
                    x.Embedding!.CosineDistance(vector))
                .Take(topK * 3)
                .ToListAsync();

        // PURE SQL keyword search
        var keywordResults =
            await _dbContext.DocumentChunks
                .Where(x =>
                    x.DocumentId == documentId &&
                    x.SearchVector.Contains(
                        normalizedQuestion))
                .Take(topK * 3)
                .ToListAsync();

        // IN-MEMORY merge + rerank
        var combined =
            semanticResults
                .Concat(keywordResults)
                .DistinctBy(x => x.Id)
                .ToList();

        var reranked =
        combined
        .Select(x => new SearchResult
        {
            ChunkId = x.Id,
            PageNumber = x.PageNumber,
            Content = x.Content,

            Similarity =
                x.SearchVector.Contains(
                    normalizedQuestion)
                    ? 1.0
                    : 0.7
        })
        .OrderByDescending(x =>
            x.Similarity)
        .Take(topK)
        .ToList();

        return reranked;
    }

    private double CalculateHybridScore(
        DocumentChunk chunk,
        string question,
        Vector vector)
    {
        var semanticScore =
            1 - chunk.Embedding!
                .CosineDistance(vector);

        var keywordScore =
            chunk.SearchVector.Contains(question)
                ? 0.3
                : 0;

        return semanticScore + keywordScore;
    }
}