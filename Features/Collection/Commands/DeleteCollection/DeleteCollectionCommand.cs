using MediatR;

namespace BackEnd.Features.Collection.Commands.DeleteCollection;

public class DeleteCollectionCommand : IRequest<bool>
{
    public int Id { get; set; }
    public Guid SyncId { get; set; }

    public DeleteCollectionCommand(int id, Guid syncId)
    {
        Id = id;
        SyncId = syncId;
    }
}