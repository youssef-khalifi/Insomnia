using BackEnd.Data;
using BackEnd.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Features.Request.Commands.DeleteRequest;

public class DeleteRequestCommandHandler : IRequestHandler<DeleteRequestCommand, bool>
{
    private readonly SqliteDbContext _sqliteContext;
    private readonly ISyncService _syncService;
    private readonly ILogger<DeleteRequestCommandHandler> _logger;

    public DeleteRequestCommandHandler(SqliteDbContext sqliteContext, ISyncService syncService, ILogger<DeleteRequestCommandHandler> logger)
    {
        _sqliteContext = sqliteContext;
        _syncService = syncService;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _sqliteContext.Requests
                .FirstOrDefaultAsync(r => r.SyncId == request.SyncId, cancellationToken);


            if (product == null)
            {
                _logger.LogWarning($"Request with ID {request.Id} not found for deletion");
                return false;
            }

            var result = await _syncService.DeleteRequest(request.SyncId);

            if (result)
            {
                _logger.LogInformation($"Deleted Request with ID {request.Id}");
            }
            else
            {
                _logger.LogWarning($"Failed to delete Request with ID {request.Id}");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting Request with ID {request.Id}");
            throw;
        }
    }
}