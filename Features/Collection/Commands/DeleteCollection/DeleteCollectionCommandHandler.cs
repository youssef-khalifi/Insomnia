using BackEnd.Data;
using BackEnd.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Features.Collection.Commands.DeleteCollection;

public class DeleteCollectionCommandHandler : IRequestHandler<DeleteCollectionCommand, bool>
{
    
    private readonly SqliteDbContext _sqliteContext;
    private readonly ISyncService _syncService;
    private readonly ILogger<DeleteCollectionCommandHandler> _logger;

    public DeleteCollectionCommandHandler(SqliteDbContext sqliteContext, ISyncService syncService, ILogger<DeleteCollectionCommandHandler> logger)
    {
        _sqliteContext = sqliteContext;
        _syncService = syncService;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteCollectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _sqliteContext.Collections.FirstOrDefaultAsync(c => c.SyncId == request.SyncId, cancellationToken);

            if (product == null)
            {
                _logger.LogWarning($"Collection with ID {request.Id} not found for deletion");
                return false;
            }

            var result = await _syncService.DeleteCollection(request.SyncId);

            if (result)
            {
                _logger.LogInformation($"Deleted Collection with ID {request.Id}");
            }
            else
            {
                _logger.LogWarning($"Failed to delete Collection with ID {request.Id}");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting Collection with ID {request.Id}");
            throw;
        }
    }
}