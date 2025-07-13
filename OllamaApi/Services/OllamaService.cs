using System.Text.Json;
using OllamaApi.Models;
using Microsoft.Extensions.Logging;

namespace OllamaApi.Services;

public interface IOllamaService
{
    Task<OllamaResponse> GenerateAsync(OllamaRequest request);
    Task<OllamaResponse> ChatAsync(OllamaChatRequest request);
    Task<List<string>> ListModelsAsync();
}

public class OllamaService : IOllamaService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<OllamaService> _logger;
    private const string ClientName = "OllamaApi";  // Match the name used in Program.cs

    public OllamaService(IHttpClientFactory httpClientFactory, ILogger<OllamaService> logger)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private HttpClient CreateClient() => _httpClientFactory.CreateClient(ClientName);

    public async Task<OllamaResponse> GenerateAsync(OllamaRequest request)
    {
        try
        {
            using var client = CreateClient();
            var response = await client.PostAsJsonAsync("generate", request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
            return result ?? throw new InvalidOperationException("Failed to deserialize response");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error generating response for prompt: {Prompt}", request.Prompt);
            throw;
        }
    }

    public async Task<OllamaResponse> ChatAsync(OllamaChatRequest request)
    {
        try
        {
            using var client = CreateClient();
            var response = await client.PostAsJsonAsync("chat", request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
            return result ?? throw new InvalidOperationException("Failed to deserialize response");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error in chat communication");
            throw;
        }
    }

    public async Task<List<string>> ListModelsAsync()
    {
        try
        {
            using var client = CreateClient();
            var response = await client.GetAsync("tags");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            if (result.ValueKind != JsonValueKind.Object)
                throw new InvalidOperationException("Invalid response format");
                
            return result.GetProperty("models").EnumerateArray()
                .Select(x => x.GetProperty("name").GetString() ?? string.Empty)
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error listing models");
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error parsing model list response");
            throw;
        }
    }
}
