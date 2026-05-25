using ClauseAI.Application.Interfaces;
using ClauseAI.Application.Models;

namespace ClauseAI.Infrastructure.AI;

public class TextChunker : ITextChunker
{
    public List<TextChunk> Chunk(
        string text,
        int pageNumber,
        int chunkSize = 1000,
        int overlap = 200)
    {
        var chunks = new List<TextChunk>();

        if (string.IsNullOrWhiteSpace(text))
        {
            return chunks;
        }

        var start = 0;
        var chunkIndex = 0;

        while (start < text.Length)
        {
            var length = Math.Min(chunkSize, text.Length - start);

            var chunkText = text.Substring(start, length);

            chunks.Add(new TextChunk
            {
                ChunkIndex = chunkIndex,
                PageNumber = pageNumber,
                Content = chunkText
            });

            start += chunkSize - overlap;

            chunkIndex++;
        }

        return chunks;
    }
}