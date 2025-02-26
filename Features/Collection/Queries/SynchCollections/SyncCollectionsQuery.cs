using BackEnd.Models;
using MediatR;

namespace BackEnd.Features.Collection.Queries.SynchCollections;

public class SyncCollectionsQuery : IRequest<SyncResult>
{
    public bool SyncNow { get; set; }

    public SyncCollectionsQuery(bool syncNow = false)
    {
        SyncNow = syncNow;
    }
}