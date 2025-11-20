using AutoMapper;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Common.Models;
using HotelBookingSystem.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelBookingSystem.Infrastructure.Identity.Services;

/// <summary>
/// Service class responsible for handling identity-related operations such as user registration, login, and email confirmation.
/// </summary>
public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the  class.
    /// </summary>
    /// <param name="userManager">The user manager instance.</param>
    /// <param name="signInManager">The sign-in manager instance.</param>
    /// <param name="jwtTokenGenerator">The JWT token generator instance.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public IdentityService(UserManager<User> userManager,
        IJwtTokenGenerator jwtTokenGenerator,
        IMapper mapper,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
        _signInManager = signInManager;
    }

    /// <summary>
    /// Confirms a user's email address using a token.
    /// </summary>
    /// <param name="email">The user's email.</param>
    /// <param name="token">The email confirmation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task ConfirmEmailAsync(string email, string token)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Logs in an existing user and returns a JWT authentication result on success.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The user's password.</param>
    /// <returns>An AuthResult indicating success or failure and containing a JWT token if successful.</returns>
    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return new AuthResult
            {
                Succeeded = false,
                Error = "Invalid email or password."
            };
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);

        if (!signInResult.Succeeded)
        {
            return new AuthResult
            {
                Succeeded = false,
                Error = "Invalid email or password."
            };
        }

        if (!user.EmailConfirmed)
        {
            return new AuthResult
            {
                Succeeded = false,
                Error = "Email is not confirmed. Please verify your email before logging in."
            };
        }

        var token = await GenerateJwtAsync(user);

        return new AuthResult
        {
            Succeeded = true,
            UserId = user.Id,
            Token = token
        };
    }

    /// <summary>
    /// Registers a new user, assigns the "User" role, and returns a JWT authentication result on success.
    /// </summary>
    /// <param name="registerUserModel">The model containing registration details.</param>
    /// <returns>An AuthResult indicating success or failure and containing a JWT token if successful.</returns>
    public async Task<AuthResult> RegisterAsync(RegisterUserModel registerUserModel)
    {
        var existing = await _userManager.FindByEmailAsync(registerUserModel.Email);

        if (existing != null)
        {
            return new AuthResult
            {
                Succeeded = false,
                Error = "The email is used."
            };
        }

        var user = _mapper.Map<User>(registerUserModel);

        var result = await _userManager.CreateAsync(user, registerUserModel.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return new AuthResult
            {
                Succeeded = false,
                Error = errors
            };
        }

        await _userManager.AddToRoleAsync(user, "User");
        //await SendConfirmationEmailAsync(user.Email!);

        var token = await GenerateJwtAsync(user);

        return new AuthResult
        {
            Succeeded = true,
            UserId = user.Id,
            Token = token
        };
    }

    /// <summary>
    /// Resets a user's password.
    /// </summary>
    /// <param name="email">The user's email.</param>
    /// <param name="token">The password reset token.</param>
    /// <param name="newPassword">The new password.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task ResetPasswordAsync(string email, string token, string newPassword)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Sends a confirmation email to the user.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SendConfirmationEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Sends a password reset email to the user.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SendPasswordResetEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    private async Task<string> GenerateJwtAsync(User user)
    {
        var authUser = _mapper.Map<AuthenticatedUser>(user);

        var claims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        return _jwtTokenGenerator.GenerateToken(authUser, claims, roles);
    }
}
