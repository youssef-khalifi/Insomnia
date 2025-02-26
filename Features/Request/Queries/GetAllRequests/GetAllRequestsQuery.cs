using BackEnd.Models;
using MediatR;

namespace BackEnd.Features.Request.Queries.GetAllRequests;

public class GetAllRequestsQuery : IRequest<IEnumerable<RequestApi>>
{
    
}