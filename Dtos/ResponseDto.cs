using System.Net;
using System.Text.Json;

namespace BackEnd.Dtos;

public class ResponseDto
{
    public HttpStatusCode StatusCode { get; set; }
    public long ResponseTime { get; set; }
    public string Body { get; set; } = string.Empty;
}