using BackEnd.Data;
using BackEnd.Models;
using MediatR;

namespace BackEnd.Features.Request.CreateRequest;

public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand , RequestApi>
{
    
    private readonly SqliteDbContext _sqliteContext;
    private readonly ILogger<CreateRequestCommandHandler> _logger;


    public CreateRequestCommandHandler(SqliteDbContext sqliteContext, ILogger<CreateRequestCommandHandler> logger)
    {
        _sqliteContext = sqliteContext;
        _logger = logger;
    }

    public async Task<RequestApi> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        
        try
        {
            var collection = await _sqliteContext.Collections.FindAsync(request.CollectionId);
            if (collection == null)
            {
                throw new Exception($"Collection with ID {request.CollectionId} does not exist.");
            }
            
            var savedRequest = new RequestApi
            {
                Name = request.Name,
                Url = request.Url,
                Method = request.Method,
                Body = request.Body,
                CollectionId = request.CollectionId,
                CreatedAt = DateTime.UtcNow,
                IsSynced = false
            };

            await _sqliteContext.Requests.AddAsync(savedRequest, cancellationToken);
            await _sqliteContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Created new Request with ID {savedRequest.Id}");

            return savedRequest;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating new request");
            throw;
        }
    }
}