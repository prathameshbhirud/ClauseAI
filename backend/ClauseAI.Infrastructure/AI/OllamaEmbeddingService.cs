using System.Net.Http.Json;
using ClauseAI.Application.Interfaces;

namespace ClauseAI.Infrastructure.AI;

public class OllamaEmbeddingService : IEmbeddingService
{
    private readonly HttpClient _httpClient;

    public OllamaEmbeddingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<float[]> GenerateAsync(string text)
    {
        var request = new
        {
            model = "nomic-embed-text",
            input = text
        };

        Console.WriteLine(_httpClient.BaseAddress);
        var response = await _httpClient.PostAsJsonAsync(
            "/api/embed",
            request);

        Console.WriteLine(await response.Content.ReadAsStringAsync());
        response.EnsureSuccessStatusCode();

        var result =
            await response.Content.ReadFromJsonAsync<OllamaEmbeddingResponse>();

        return result?.Embeddings?.FirstOrDefault() ?? [];
    }

    private class OllamaEmbeddingResponse
    {
        public List<float[]> Embeddings { get; set; } = [];
    }
}