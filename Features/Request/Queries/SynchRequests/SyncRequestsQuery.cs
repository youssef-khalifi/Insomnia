using BackEnd.Models;
using MediatR;

namespace BackEnd.Features.Request.Queries.SyncRequests;

public class SyncRequestsQuery : IRequest<SyncResult>
{
    public bool SyncNow { get; set; }

    public SyncRequestsQuery(bool syncNow = false)
    {
        SyncNow = syncNow;
    }
}