namespace BackEnd.Dtos;

public class SendRequestDto
{
    public string Url { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}