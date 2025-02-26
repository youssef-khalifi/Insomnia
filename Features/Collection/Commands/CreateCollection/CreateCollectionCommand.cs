using BackEnd.Dtos;
using MediatR;

namespace BackEnd.Features.Collection.Commands.CreateCollection;

public class CreateCollectionCommand : IRequest<Models.Collection>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public static CreateCollectionCommand FromDto(CreateCollectionDto dto)
    {
        return new CreateCollectionCommand
        {
            Name = dto.Name,
            Description = dto.Description,
            
        };
    }
}