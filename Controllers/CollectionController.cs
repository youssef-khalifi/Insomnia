using BackEnd.Dtos;
using BackEnd.Features.Collection.Commands.CreateCollection;
using BackEnd.Features.Collection.Commands.DeleteCollection;
using BackEnd.Features.Collection.Queries.GetAllCollection;
using BackEnd.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CollectionController : ControllerBase
{
    private readonly IMediator _mediator;

    public CollectionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCollection([FromBody] CreateCollectionDto collectionDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = CreateCollectionCommand.FromDto(collectionDto);
        var createdCollection = await _mediator.Send(command);

        return Ok(collectionDto);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Collection>>> GetCollections()
    {
        var products = await _mediator.Send(new GetAllCollectionsQuery());
        return Ok(products);
    }
    
    [HttpDelete("{id}/{syncId}")]
    public async Task<IActionResult> DeleteCollection([FromRoute] int id, [FromRoute] Guid syncId)
    {
        // Check if the provided SyncId is valid (not empty)
        if (syncId == Guid.Empty)
        {
            return BadRequest("Invalid SyncId provided.");
        }

        // Send the delete command to the mediator
        var result = await _mediator.Send(new DeleteCollectionCommand(id, syncId));

        // Check if the deletion was successful or if the request wasn't found
        if (!result)
        {
            return NotFound(new { Message = $"Collection with ID {id} and SyncId {syncId} not found." });
        }

        // Return a success message if deletion was successful
        return Ok(new { Message = $"Collection with ID {id} and SyncId {syncId} has been deleted." });
    }
}