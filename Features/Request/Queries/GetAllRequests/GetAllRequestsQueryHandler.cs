using BackEnd.Data;
using BackEnd.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Features.Request.Queries.GetAllRequests;

public class GetAllRequestsQueryHandler : IRequestHandler<GetAllRequestsQuery , IEnumerable<RequestApi>>
{
    
    private readonly SqliteDbContext _sqliteContext;
    private readonly ILogger<GetAllRequestsQueryHandler> _logger;

    public GetAllRequestsQueryHandler(SqliteDbContext sqliteContext, ILogger<GetAllRequestsQueryHandler> logger)
    {
        _sqliteContext = sqliteContext;
        _logger = logger;
    }

    public async Task<IEnumerable<RequestApi>> Handle(GetAllRequestsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _sqliteContext.Requests.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all requets");
            throw;
        }
    }
}