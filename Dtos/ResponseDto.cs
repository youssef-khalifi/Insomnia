using System.Text.Json;

namespace BackEnd.Dtos;

public class ResponseDto
{
    public int StatusCode { get; set; }
    public int ResponseTime { get; set; }
    public string Body { get; set; } = string.Empty;
}