using HotelBookingSystem.Application.Features.Authentication.Commands.EmailConfirmation;
using HotelBookingSystem.Application.Features.Authentication.Commands.Login;
using HotelBookingSystem.Application.Features.Authentication.Commands.PasswordReset;
using HotelBookingSystem.Application.Features.Authentication.Commands.Register;
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

    /// <summary>
    /// Requests the server to generate and send a new email confirmation link to the specified address.
    /// Used if the initial email was not received.
    /// </summary>
    /// <param name="command">The command containing the email address.</param>
    /// <returns>Always returns 200 OK (No Content) for security reasons.</returns>
    [HttpPost("send-confirmation")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SendConfirmation([FromBody] SendConfirmationEmailCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    /// <summary>
    /// Confirms the user's email address using the token received in the email link.
    /// </summary>
    /// <param name="command">The command containing the user's email and the confirmation token.</param>
    /// <returns>Success status (200 OK) upon successful confirmation.</returns>
    [HttpPost("confirm-email")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    /// <summary>
    /// Initiates the password reset process. Requests the server to generate and send a password reset link to the specified email.
    /// </summary>
    /// <param name="command">The command containing the email address.</param>
    /// <returns>Always returns 200 OK (No Content) for security reasons.</returns>
    [HttpPost("forgot-password")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    /// <summary>
    /// Resets the user's password using the token received from the forgot-password email.
    /// </summary>
    /// <param name="command">The command containing email, token, and the new password.</param>
    /// <returns>Success status (200 OK) upon successful password reset.</returns>
    [HttpPost("reset-password")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        // The handler is responsible for throwing an IdentityException if the reset fails
        await _mediator.Send(command);
        return Ok();
    }
}
