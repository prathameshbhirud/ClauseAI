namespace ClauseAI.Application.Interfaces;

public interface IChatCompletionService
{
    Task<string> AskAsync(
        string question,
        string context);
}