using System.Text.Json;
using BackEnd.Dtos;
using BackEnd.Models;
using MediatR;

namespace BackEnd.Features.Request.CreateRequest;

public class CreateRequestCommand : IRequest<RequestApi>
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int CollectionId { get; set; } // Foreign Key
    
   
    
    public static CreateRequestCommand FromDto(CreateRequestDto dto)
    {
        return new CreateRequestCommand
        {
            Name = dto.Name,
            Url = dto.Url,
            Method = dto.Method,
            Body = dto.Body,
            CollectionId = dto.CollectionId,
            
            
            
        };
    }
}