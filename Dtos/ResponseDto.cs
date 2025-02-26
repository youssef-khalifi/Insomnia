using System.Text.Json;

namespace BackEnd.Dtos;

public class ResponseDto
{
    public int StatusCode { get; set; }
    public int ResponseTime { get; set; }
    public string Body { get; set; } = string.Empty;
    public string HeadersJson { get; set; } = string.Empty;
    public Dictionary<string, string> Headers
    {
        get => HeadersJson == null ? new Dictionary<string, string>() : JsonSerializer.Deserialize<Dictionary<string, string>>(HeadersJson);
        set => HeadersJson = JsonSerializer.Serialize(value);
    }
}