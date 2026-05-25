using ClauseAI.Application.Models;

namespace ClauseAI.Application.Interfaces;

public interface IVectorSearchService
{
    Task<List<SearchResult>> SearchAsync(
        Guid documentId,
        string question,
        int topK = 5);
}