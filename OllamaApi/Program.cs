using Microsoft.OpenApi.Models;
using OllamaApi.Models;
using OllamaApi.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "Ollama API", 
        Version = "v1",
        Description = "API for interacting with Ollama LLM models"
    });
});

// Register Ollama service and configure HttpClient
builder.Services.AddHttpClient("OllamaApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:11434/api/");
    client.Timeout = TimeSpan.FromMinutes(5);
})
.ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
{
    PooledConnectionLifetime = TimeSpan.FromMinutes(15),
    KeepAlivePingDelay = TimeSpan.FromSeconds(30),
    KeepAlivePingTimeout = TimeSpan.FromSeconds(5),
    EnableMultipleHttp2Connections = true
})
.SetHandlerLifetime(Timeout.InfiniteTimeSpan);

builder.Services.AddScoped<IOllamaService, OllamaService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Static endpoint handlers with TypedResults
static async Task<IResult> GenerateText(IOllamaService service, OllamaRequest request)
{
    try
    {
        var response = await service.GenerateAsync(request);
        var dto = OllamaResponseDto.FromResponse(response);
        return TypedResults.Ok(dto);
    }
    catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
    {
        return TypedResults.NotFound(new { message = "Model not found" });
    }
    catch (Exception ex)
    {
        return TypedResults.Problem(ex.Message);
    }
}

static async Task<IResult> Chat(IOllamaService service, OllamaChatRequest request)
{
    try
    {
        var response = await service.ChatAsync(request);
        var dto = OllamaResponseDto.FromResponse(response);
        return TypedResults.Ok(dto);
    }
    catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
    {
        return TypedResults.NotFound(new { message = "Model not found" });
    }
    catch (Exception ex)
    {
        return TypedResults.Problem(ex.Message);
    }
}

static async Task<IResult> ListModels(IOllamaService service)
{
    try
    {
        var models = await service.ListModelsAsync();
        return TypedResults.Ok(models);
    }
    catch (Exception ex)
    {
        return TypedResults.Problem(ex.Message);
    }
}

// Group Ollama endpoints
var ollamaApi = app.MapGroup("/api/ollama")
    .WithTags("Ollama")
    .WithOpenApi();

// Map endpoints to handlers
ollamaApi.MapPost("/generate", GenerateText)
    .WithName("Generate")
    .WithDescription("Generate text using an Ollama model")
    .WithSummary("Generate text");

ollamaApi.MapPost("/chat", Chat)
    .WithName("Chat")
    .WithDescription("Chat with an Ollama model")
    .WithSummary("Chat conversation");

ollamaApi.MapGet("/models", ListModels)
    .WithName("ListModels")
    .WithDescription("List all available Ollama models")
    .WithSummary("List models");

app.Run();
