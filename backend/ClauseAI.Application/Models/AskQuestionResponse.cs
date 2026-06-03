using ClauseAI.Application.DTOs;

namespace ClauseAI.Application.Models;

public class AskQuestionResponse
{
    public string Answer { get; set; } = string.Empty;

    public List<CitationDto> Citations { get; set; } = [];
}