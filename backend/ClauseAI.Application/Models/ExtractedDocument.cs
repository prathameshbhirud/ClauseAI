namespace ClauseAI.Application.Models;

public class ExtractedDocument
{
    public Guid DocumentId { get; set; }

    public List<ExtractedPage> Pages { get; set; } = [];
}