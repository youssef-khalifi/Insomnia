using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services;

public interface ISyncService
{
    Task<SyncResult> SyncCollectionsToSqlServer();
    Task<SyncResult> SyncResponseToSqlServer();
    Task<SyncResult> SyncRequestToSqlServer();
    Task<bool> DeleteCollection(Guid collectionSyncId);
    Task<bool> DeleteRequest(Guid requestSyncId);
    Task<bool> DeleteResponse(int responseId);
}