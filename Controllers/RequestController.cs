using BackEnd.Dtos;
using BackEnd.Features.Request.Commands.DeleteRequest;
using BackEnd.Features.Request.CreateRequest;
using BackEnd.Features.Request.Queries.GetAllRequests;
using BackEnd.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RequestController : ControllerBase
{
    private readonly IMediator _mediator;

    public RequestController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCollection([FromBody] CreateRequestDto requestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = CreateRequestCommand.FromDto(requestDto);
        var createdRequest = await _mediator.Send(command);

        return Ok(new RequestDto
        {
            Name = createdRequest.Name,
            Url = createdRequest.Url,
            Method = createdRequest.Method,
            CollectionId = createdRequest.CollectionId,
            Body = createdRequest.Body,
        });
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RequestApi>>> GetProducts()
    {
        var requests = await _mediator.Send(new GetAllRequestsQuery());
        return Ok(requests);
    }
    [HttpDelete("{id}/{syncId}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] int id, [FromRoute] Guid syncId)
    {
        // Check if the provided SyncId is valid (not empty)
        if (syncId == Guid.Empty)
        {
            return BadRequest("Invalid SyncId provided.");
        }

        // Send the delete command to the mediator
        var result = await _mediator.Send(new DeleteRequestCommand(id, syncId));

        // Check if the deletion was successful or if the request wasn't found
        if (!result)
        {
            return NotFound(new { Message = $"Request with ID {id} and SyncId {syncId} not found." });
        }

        // Return a success message if deletion was successful
        return Ok(new { Message = $"Request with ID {id} and SyncId {syncId} has been deleted." });
    }

}