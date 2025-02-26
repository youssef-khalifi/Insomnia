using MediatR;

namespace BackEnd.Features.Request.Commands.DeleteRequest;

public class DeleteRequestCommand : IRequest<bool>
{
    public int Id { get; set; }
    public Guid SyncId { get; set; }
    
    public DeleteRequestCommand(int id , Guid syncId)
    {
        Id = id;
        SyncId = syncId;
    }
}