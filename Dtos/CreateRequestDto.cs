using System.Text.Json;
using System.Text.Json.Serialization;

namespace BackEnd.Dtos;

public class CreateRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int CollectionId { get; set; } // Foreign Key
    
   
}