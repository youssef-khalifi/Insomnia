using BackEnd.Dtos;
using BackEnd.Features.SendRequest.getRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HttpController : ControllerBase
{
    
    private readonly IMediator _mediator;

    public HttpController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Send([FromBody] SendRequestDto requestDto)
    {
        var requestCommand = SendRequestCommand.FromDto(requestDto);
        
        var response = _mediator.Send(requestCommand);
        return Ok(new ResponseDto
        {
            StatusCode = response.Result.StatusCode,
            ResponseTime = response.Result.ResponseTime,
            Body = response.Result.Body,
        });
    }
}