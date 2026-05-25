namespace ClauseAI.Application.Models;

public class TextChunk
{
    public int ChunkIndex { get; set; }

    public int PageNumber { get; set; }

    public string Content { get; set; } = string.Empty;
}