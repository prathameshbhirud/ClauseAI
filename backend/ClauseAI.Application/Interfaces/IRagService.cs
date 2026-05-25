using ClauseAI.Application.Models;

namespace ClauseAI.Application.Interfaces;

public interface IRagService
{
    Task<AskQuestionResponse> AskAsync(
        Guid documentId,
        string question,
        int topK = 5);
}