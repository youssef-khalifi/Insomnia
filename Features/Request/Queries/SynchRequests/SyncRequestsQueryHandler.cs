using BackEnd.Models;
using BackEnd.Services;
using MediatR;

namespace BackEnd.Features.Request.Queries.SyncRequests;

public class SyncRequestsQueryHandler : IRequestHandler<SyncRequestsQuery, SyncResult>
{
    private readonly ISyncService _syncService;
    private readonly ILogger<SyncRequestsQueryHandler> _logger;

    public SyncRequestsQueryHandler(ISyncService syncService, ILogger<SyncRequestsQueryHandler> logger)
    {
        _syncService = syncService;
        _logger = logger;
    }

    public async Task<SyncResult> Handle(SyncRequestsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!request.SyncNow)
            {
                return new SyncResult
                {
                    Success = true,
                    Message = "Sync not triggered. Pass syncNow=true to trigger synchronization."
                };
            }

            return await _syncService.SyncRequestToSqlServer();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during synchronization");
            return new SyncResult
            {
                Success = false,
                Message = $"Sync failed: {ex.Message}"
            };
        }
    }
}