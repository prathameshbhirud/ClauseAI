using ClauseAI.Application.DTOs;
using ClauseAI.Application.Interfaces;
using ClauseAI.Application.Models;

namespace ClauseAI.Infrastructure.AI;

public class RagService : IRagService
{
    private readonly IVectorSearchService _vectorSearchService;
    private readonly IChatCompletionService _chatCompletionService;

    public RagService(
        IVectorSearchService vectorSearchService,
        IChatCompletionService chatCompletionService)
    {
        _vectorSearchService = vectorSearchService;
        _chatCompletionService = chatCompletionService;
    }

    public async Task<AskQuestionResponse> AskAsync(
        Guid documentId,
        string question,
        int topK = 5)
    {
        var chunks =
            await _vectorSearchService.SearchAsync(
                documentId,
                question,
                topK);

        var context = string.Join(
            "\n\n",
            chunks.Select(x =>
                $"[Page {x.PageNumber}]\n{x.Content}"));

        var answer =
            await _chatCompletionService.AskAsync(
                question,
                context);

        return new AskQuestionResponse
        {
            Answer = answer,
            Citations = chunks.Select(x => new CitationDto
            {
                PageNumber = x.PageNumber,
                Content = x.Content
            }).ToList()
        };
    }
}