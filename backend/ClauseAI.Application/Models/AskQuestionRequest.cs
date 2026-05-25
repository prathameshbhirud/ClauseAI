namespace ClauseAI.Application.Models;

public class AskQuestionRequest
{
    public string Question { get; set; } = string.Empty;

    public int TopK { get; set; } = 5;
}