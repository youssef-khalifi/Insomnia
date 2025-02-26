using BackEnd.Features.Collection.Queries.SynchCollections;
using BackEnd.Features.Request.Queries.SyncRequests;
using BackEnd.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SyncController : ControllerBase
{
    private readonly IMediator _mediator;

    public SyncController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("syncCollection")]
    public async Task<IActionResult> SyncProducts([FromQuery] bool syncNow = false)
    {
        var result = await _mediator.Send(new SyncCollectionsQuery(syncNow));

        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [HttpGet("syncRequest")]
    public async Task<IActionResult> SyncReq([FromQuery] bool syncNow = false)
    {
        var result = await _mediator.Send(new SyncRequestsQuery(syncNow));

        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}