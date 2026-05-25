using ClauseAI.Application.Models;

namespace ClauseAI.Application.Interfaces;

public interface ITextChunker
{
    List<TextChunk> Chunk(
        string text,
        int pageNumber,
        int chunkSize = 1000,
        int overlap = 200);
}