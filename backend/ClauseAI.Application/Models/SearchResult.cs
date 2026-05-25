namespace ClauseAI.Application.Models;

public class SearchResult
{
    public Guid ChunkId { get; set; }

    public int PageNumber { get; set; }

    public string Content { get; set; } = string.Empty;

    public double Similarity { get; set; }
}