namespace ClauseAI.Application.Interfaces;

public interface IEmbeddingService
{
    Task<float[]> GenerateAsync(string text);
}