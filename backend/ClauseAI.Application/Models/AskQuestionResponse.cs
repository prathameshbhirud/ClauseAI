namespace ClauseAI.Application.Models;

public class AskQuestionResponse
{
    public string Answer { get; set; } = string.Empty;

    public List<Citation> Citations { get; set; } = [];
}

public class Citation
{
    public int PageNumber { get; set; }

    public string Content { get; set; } = string.Empty;
}