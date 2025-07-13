namespace OllamaApi.Models;

public class OllamaResponseDto
{
    public string Model { get; set; } = string.Empty;
    public string Response { get; set; } = string.Empty;
    public bool Done { get; set; }

    public static OllamaResponseDto FromResponse(OllamaResponse response) =>
        new()
        {
            Model = response.Model,
            Response = response.Response,
            Done = response.Done
        };
}
