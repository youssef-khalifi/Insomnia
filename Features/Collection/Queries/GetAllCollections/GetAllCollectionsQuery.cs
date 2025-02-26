using MediatR;

namespace BackEnd.Features.Collection.Queries.GetAllCollection;

public class GetAllCollectionsQuery : IRequest<IEnumerable<Models.Collection>>
{
    
}