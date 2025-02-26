using BackEnd.Data;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services;

public class SyncService : ISyncService
{
    private readonly SqliteDbContext _sqliteContext;
    private readonly SqlServerDbContext _sqlServerContext;
    private readonly ILogger<SyncService> _logger;


    public SyncService(SqliteDbContext sqliteContext, SqlServerDbContext sqlServerContext, ILogger<SyncService> logger)
    {
        _sqliteContext = sqliteContext;
        _sqlServerContext = sqlServerContext;
        _logger = logger;
    }

    public async Task<SyncResult> SyncCollectionsToSqlServer()
    {
       var result = new SyncResult { Success = true };

            try
            {
                // Get all products from SQLite that need syncing (including deleted ones)
                var sqliteCollections = await _sqliteContext.Collections
                    .IgnoreQueryFilters() // Include soft-deleted items
                    .Where(p => !p.IsSynced)
                    .ToListAsync();

                foreach (var sqliteCollection in sqliteCollections)
                {
                    // Check if product exists in SQL Server
                    var sqlServerCollection = await _sqlServerContext.Collections
                        .IgnoreQueryFilters() // Include soft-deleted items
                        .FirstOrDefaultAsync(p => p.SyncId == sqliteCollection.SyncId);

                    if (sqlServerCollection == null)
                    {
                        // Product doesn't exist in SQL Server, add it
                        if (!sqliteCollection.IsDeleted)
                        {
                            var newCollection = new Collection
                            {
                                SyncId = sqliteCollection.SyncId,
                                Name = sqliteCollection.Name,
                                Description = sqliteCollection.Description,
                                CreatedAt = sqliteCollection.CreatedAt,
                                UpdatedAt = sqliteCollection.UpdatedAt,
                                IsDeleted = sqliteCollection.IsDeleted
                            };

                            await _sqlServerContext.Collections.AddAsync(newCollection);
                            result.Added++;
                        }
                    }
                    else
                    {
                        // Product exists in SQL Server, update it
                        if (sqliteCollection.IsDeleted)
                        {
                            // Mark as deleted in SQL Server if deleted in SQLite
                            sqlServerCollection.IsDeleted = true;
                            sqlServerCollection.UpdatedAt = DateTime.UtcNow;
                            result.Deleted++;
                        }
                        else
                        {
                            // Update existing record
                            sqlServerCollection.Name = sqliteCollection.Name;
                            sqlServerCollection.Description = sqliteCollection.Description;
                            sqlServerCollection.UpdatedAt = DateTime.UtcNow;
                            result.Updated++;
                        }

                        _sqlServerContext.Collections.Update(sqlServerCollection);
                    }

                    // Mark as synced in SQLite
                    sqliteCollection.IsSynced = true;
                    sqliteCollection.LastSyncedAt = DateTime.UtcNow;
                    _sqliteContext.Collections.Update(sqliteCollection);
                }

                // Save changes to both databases
               // await _sqlServerContext.SaveChangesAsync();
              //  await _sqliteContext.SaveChangesAsync();
              
              var sqlServerTransaction = await _sqlServerContext.Database.BeginTransactionAsync();
              var sqliteTransaction = await _sqliteContext.Database.BeginTransactionAsync();

              try
              {
                  await _sqlServerContext.SaveChangesAsync();
                  await _sqliteContext.SaveChangesAsync();
    
                  await sqlServerTransaction.CommitAsync();
                  await sqliteTransaction.CommitAsync();
              }
              catch
              {
                  await sqlServerTransaction.RollbackAsync();
                  await sqliteTransaction.RollbackAsync();
                  throw;
              }

              
                result.Message = $"Sync completed successfully. Added: {result.Added}, Updated: {result.Updated}, Deleted: {result.Deleted}";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Sync failed: {ex.Message}";
                _logger.LogError(ex, "Error during database synchronization");
            }

            return result;
    }

    public Task<SyncResult> SyncResponseToSqlServer()
    {
        throw new NotImplementedException();
    }

    public async Task<SyncResult> SyncRequestToSqlServer()
    {
        var result = new SyncResult { Success = true };

            try
            {
                
                var sqliteRequests = await _sqliteContext.Requests
                    .IgnoreQueryFilters() // Include soft-deleted items
                    .Where(p => !p.IsSynced)
                    .ToListAsync();

                foreach (var sqliteRequest in sqliteRequests)
                {
                    var sqlServerRequest = await _sqlServerContext.Requests
                        .IgnoreQueryFilters() // Include soft-deleted items
                        .FirstOrDefaultAsync(p => p.SyncId == sqliteRequest.SyncId);

                    if (sqlServerRequest == null)
                    {
                        // Product doesn't exist in SQL Server, add it
                        if (!sqliteRequest.IsDeleted)
                        {
                            var newRequest = new RequestApi
                            {
                                SyncId = sqliteRequest.SyncId,
                                Name = sqliteRequest.Name,
                                Url = sqliteRequest.Url,
                                Method = sqliteRequest.Method,
                                Body = sqliteRequest.Body,
                                CreatedAt = sqliteRequest.CreatedAt,
                                UpdatedAt = sqliteRequest.UpdatedAt,
                                IsDeleted = sqliteRequest.IsDeleted
                            };

                            await _sqlServerContext.Requests.AddAsync(newRequest);
                            result.Added++;
                        }
                    }
                    else
                    {
                        // Product exists in SQL Server, update it
                        if (sqliteRequest.IsDeleted)
                        {
                            // Mark as deleted in SQL Server if deleted in SQLite
                            sqlServerRequest.IsDeleted = true;
                            sqlServerRequest.UpdatedAt = DateTime.UtcNow;
                            result.Deleted++;
                        }
                        else
                        {
                            // Update existing record
                            sqliteRequest.SyncId = sqliteRequest.SyncId;
                            sqliteRequest.Name = sqliteRequest.Name;
                            sqlServerRequest.Url = sqliteRequest.Url;
                            sqlServerRequest.Method = sqliteRequest.Method;
                            sqlServerRequest.Body = sqliteRequest.Body;
                            sqlServerRequest.CollectionId = sqliteRequest.CollectionId;
                            sqlServerRequest.UpdatedAt = DateTime.UtcNow;
                            
                            result.Updated++;
                        }

                        _sqlServerContext.Requests.Update(sqlServerRequest);
                    }

                    // Mark as synced in SQLite
                    sqliteRequest.IsSynced = true;
                    sqliteRequest.LastSyncedAt = DateTime.UtcNow;
                    _sqliteContext.Requests.Update(sqliteRequest);
                }

                // Save changes to both databases
               // await _sqlServerContext.SaveChangesAsync();
              //  await _sqliteContext.SaveChangesAsync();
              
              var sqlServerTransaction = await _sqlServerContext.Database.BeginTransactionAsync();
              var sqliteTransaction = await _sqliteContext.Database.BeginTransactionAsync();

              try
              {
                  await _sqlServerContext.SaveChangesAsync();
                  await _sqliteContext.SaveChangesAsync();
    
                  await sqlServerTransaction.CommitAsync();
                  await sqliteTransaction.CommitAsync();
              }
              catch
              {
                  await sqlServerTransaction.RollbackAsync();
                  await sqliteTransaction.RollbackAsync();
                  throw;
              }

              
                result.Message = $"Sync completed successfully. Added: {result.Added}, Updated: {result.Updated}, Deleted: {result.Deleted}";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Sync failed: {ex.Message}";
                _logger.LogError(ex, "Error during database synchronization");
            }

            return result;
    }

    
    public async Task<bool> DeleteCollection(Guid collectionSyncId)
    {
        try
        {
            var sqliteTask = Task.Run(async () =>
            {
                var sqliteCollection = await _sqliteContext.Collections
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(c => c.SyncId == collectionSyncId);

                if (sqliteCollection != null)
                {
                    sqliteCollection.IsDeleted = true;
                    sqliteCollection.UpdatedAt = DateTime.UtcNow;
                    sqliteCollection.IsSynced = false;
                    await _sqliteContext.SaveChangesAsync();
                }
            });

            var sqlServerTask = Task.Run(async () =>
            {
                var sqlServerCollection = await _sqlServerContext.Requests
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(c => c.SyncId == collectionSyncId);

                if (sqlServerCollection != null)
                {
                    sqlServerCollection.IsDeleted = true;
                    sqlServerCollection.UpdatedAt = DateTime.UtcNow;
                    await _sqlServerContext.SaveChangesAsync();
                }
            });

            await Task.WhenAll(sqliteTask, sqlServerTask);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting Collection with ID {collectionSyncId}");
            return false;
        }
    }

    public async Task<bool> DeleteRequest(Guid requestSyncId)
    {
        try
        {

            var sqliteTask = Task.Run(async () =>
            {
                var sqliteRequest = await _sqliteContext.Requests
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(req => req.SyncId == requestSyncId);

                if (sqliteRequest != null)
                {
                    sqliteRequest.IsDeleted = true;
                    sqliteRequest.UpdatedAt = DateTime.UtcNow;
                    sqliteRequest.IsSynced = false;
                    await _sqliteContext.SaveChangesAsync();
                }
            });

            var sqlServerTask = Task.Run(async () =>
            {
                var sqlServerRequest = await _sqlServerContext.Requests
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(req => req.SyncId == requestSyncId);

                if (sqlServerRequest != null)
                {
                    sqlServerRequest.IsDeleted = true;
                    sqlServerRequest.UpdatedAt = DateTime.UtcNow;
                    await _sqlServerContext.SaveChangesAsync();
                }
            });

            await Task.WhenAll(sqliteTask, sqlServerTask);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting Request with ID {requestSyncId}");
            return false;
        }
    }

    public Task<bool> DeleteResponse(int responseId)
    {
        throw new NotImplementedException();
    }
    
    
    
}