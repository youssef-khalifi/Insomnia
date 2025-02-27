using BackEnd.Dtos;
using BackEnd.Models;
using MediatR;

namespace BackEnd.Features.SendRequest.getRequest;

public class SendRequestCommand : IRequest<ResponseApi>
{
    
    public string Url { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    
    public static SendRequestCommand FromDto(SendRequestDto dto)
    {
        return new SendRequestCommand
        {
            Url = dto.Url,
            Method = dto.Method,
            Body = dto.Body,
            
        };
    }
}