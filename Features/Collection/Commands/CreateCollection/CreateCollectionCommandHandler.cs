using BackEnd.Data;
using MediatR;

namespace BackEnd.Features.Collection.Commands.CreateCollection;

public class CreateCollectionCommandHandler : IRequestHandler<CreateCollectionCommand, Models.Collection>
{
    
    private readonly SqliteDbContext _sqliteContext;
    private readonly ILogger<CreateCollectionCommand> _logger;

    public CreateCollectionCommandHandler(SqliteDbContext sqliteContext, ILogger<CreateCollectionCommand> logger)
    {
        _sqliteContext = sqliteContext;
        _logger = logger;
    }

    public async Task<Models.Collection> Handle(CreateCollectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var collection = new Models.Collection
            {
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                IsSynced = false
            };

            await _sqliteContext.Collections.AddAsync(collection, cancellationToken);
            await _sqliteContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Created new collection with ID {collection.Id}");

            return collection;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating new collection");
            throw;
        }
    }
}