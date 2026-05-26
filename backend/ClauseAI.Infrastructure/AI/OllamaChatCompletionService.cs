using System.Net.Http.Json;
using ClauseAI.Application.Interfaces;
using System.Text.Json;

namespace ClauseAI.Infrastructure.AI;

public class OllamaChatCompletionService : IChatCompletionService
{
    private readonly HttpClient _httpClient;

    public OllamaChatCompletionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> AskAsync(
        string question,
        string context)
    {
        var prompt =
                    $"""
                    You are an insurance policy assistant.

                    Answer ONLY from the provided context.

                    If the answer is not present in context, say:
                    "The uploaded policy does not contain this information."

                    Provide concise and accurate answers.

                    Context:
                    {context}

                    Question:
                    {question}
                    """;

        var request = new
        {
            model = "phi3:mini",
            input = prompt,
            stream = false
        };

        Console.WriteLine("Calling Ollama...");

        var response = await _httpClient.PostAsJsonAsync(
            "/api/generate",
            request);

        Console.WriteLine(response.StatusCode);

        response.EnsureSuccessStatusCode();

        var result =
            await response.Content
                .ReadFromJsonAsync<OllamaGenerateResponse>();

        Console.WriteLine(result);

        return result?.Response ?? string.Empty;
    }

    public async IAsyncEnumerable<string> StreamAsync(
    string question,
    string context)
    {
        var prompt =
                    $"""
                    You are an insurance policy assistant.

                    Answer ONLY from the provided context.

                    If the answer is not present in context, say:
                    "The uploaded policy does not contain this information."

                    Context:
                    {context}

                    Question:
                    {question}
                    """;

        var request = new
        {
            model = "phi3:mini",
            input = prompt,
            stream = true,
            options = new
            {
                num_predict = 200
            }
        };

        Console.WriteLine("Prompt lenght  is: " + prompt.Length);

        var response =
            await _httpClient.PostAsJsonAsync(
                "/api/generate",
                request);

        response.EnsureSuccessStatusCode();

        var stream =
            await response.Content.ReadAsStreamAsync();

        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            try
            {
                var json = JsonDocument.Parse(line);

                if (json.RootElement.TryGetProperty(
                    "response",
                    out var token))
                {
                    yield return token.GetString() ?? "";
                }
            }
            finally
            {
                // Ignore malformed chunks
            }
        }
    }

    private class OllamaGenerateResponse
    {
        public string Response { get; set; } = string.Empty;
    }
}