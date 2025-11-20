using HotelBookingSystem.Application.Authentication.Commands.Login;
using HotelBookingSystem.Application.Authentication.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers;

/// <summary>
/// Controller for user authentication operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the AuthController class.
    /// </summary>   
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Registers a new user and returns a JWT token upon successful creation.
    /// </summary>
    /// <param name="command">The registration command containing user details.</param>
    /// <returns>A JWT token and user details if registration is successful.</returns>
    [HttpPost("register")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(RegisterResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Authenticates a user with email and password and returns a JWT token.
    /// </summary>
    /// <param name="command">The login command containing credentials.</param>
    /// <returns>A JWT token if authentication is successful.</returns>
    [HttpPost("login")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
