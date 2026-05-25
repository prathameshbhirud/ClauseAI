using ClauseAI.Application.Models;

namespace ClauseAI.Application.Interfaces;

public interface IPdfTextExtractor
{
    Task<ExtractedDocument> ExtractAsync(
        Guid documentId,
        string filePath);
}