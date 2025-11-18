using HotelBookingSystem.Application.Authentication.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Registers a new user and returns a JWT token.
    /// </summary>
    [HttpPost("register")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(RegisterResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
