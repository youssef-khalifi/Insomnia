using BackEnd.Models;
using BackEnd.Services;
using MediatR;

namespace BackEnd.Features.Collection.Queries.SynchCollections;

public class SyncCollectionsQueryHandler : IRequestHandler<SyncCollectionsQuery, SyncResult>
{
    private readonly ISyncService _syncService;
    private readonly ILogger<SyncCollectionsQueryHandler> _logger;

    public SyncCollectionsQueryHandler(ISyncService syncService, ILogger<SyncCollectionsQueryHandler> logger)
    {
        _syncService = syncService;
        _logger = logger;
    }

    public async Task<SyncResult> Handle(SyncCollectionsQuery request, CancellationToken cancellationToken)
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

            return await _syncService.SyncCollectionsToSqlServer();
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