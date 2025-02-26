using BackEnd.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Features.Collection.Queries.GetAllCollection;

public class GetAllCollectionsQueryHandler : IRequestHandler<GetAllCollectionsQuery , IEnumerable<Models.Collection>>
{
    
    private readonly SqliteDbContext _sqliteContext;
    private readonly ILogger<GetAllCollectionsQueryHandler> _logger;

    public GetAllCollectionsQueryHandler(SqliteDbContext sqliteContext, ILogger<GetAllCollectionsQueryHandler> logger)
    {
        _sqliteContext = sqliteContext;
        _logger = logger;
    }

    public async Task<IEnumerable<Models.Collection>> Handle(GetAllCollectionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _sqliteContext.Collections.Include(c => c.Requests)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all collections");
            throw;
        }
    }
}