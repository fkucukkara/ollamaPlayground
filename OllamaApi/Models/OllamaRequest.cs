namespace OllamaApi.Models;

public class OllamaRequest
{
    public string Model { get; set; } = "gemma3";
    public string Prompt { get; set; } = string.Empty;
}

public class OllamaChatRequest
{
    public string Model { get; set; } = "gemma3";
    public List<ChatMessage> Messages { get; set; } = new();
}

public class ChatMessage
{
    public string Role { get; set; } = "user";
    public string Content { get; set; } = string.Empty;
}
