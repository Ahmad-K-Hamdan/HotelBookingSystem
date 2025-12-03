using AutoMapper;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Common.Models;
using HotelBookingSystem.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;

namespace HotelBookingSystem.Infrastructure.Identity.Services;

/// <summary>
/// Service class responsible for handling identity-related operations such as 
/// user registration, login, email confirmation and password reset.
/// </summary>
public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IEmailService _emailService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly JwtSettings _jwtSettings;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the  class.
    /// </summary>
    /// <param name="userManager">The user manager instance.</param>
    /// <param name="signInManager">The sign-in manager instance.</param>
    /// <param name="emailService">Service responsible for sending emails.</param>
    /// <param name="jwtTokenGenerator">The JWT token generator instance.</param>
    /// <param name="jwtSettings">Configuration settings for JWT generation.</param>
    /// <param name="mapper">The AutoMapper instance.</param>           
    public IdentityService(UserManager<User> userManager,
        IJwtTokenGenerator jwtTokenGenerator,
        IMapper mapper,
        SignInManager<User> signInManager,
        IOptions<JwtSettings> jwtSettings,
        IEmailService emailService)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
        _signInManager = signInManager;
        _jwtSettings = jwtSettings.Value;
        _emailService = emailService;
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
            return new AuthResult
            {
                Succeeded = false,
                Error = string.Join("; ", result.Errors.Select(e => e.Description))
            };
        }

        await _userManager.AddToRoleAsync(user, "User");

        try
        {
            await SendConfirmationEmailAsync(user.Email!);
        }
        catch (Exception) { }

        return new AuthResult
        {
            Succeeded = true,
            UserId = user.Id
        };
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
    /// Generates an email confirmation token and sends the confirmation link to the specified email.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SendConfirmationEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return;
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encoded = Uri.EscapeDataString(token);

        var url = $"{_jwtSettings.ConfirmEmailUrl}?email={Uri.EscapeDataString(email)}&token={encoded}";

        await _emailService.SendEmailAsync(email, "Confirm your account",
            $"Click the link to confirm your account: {url}");
    }

    /// <summary>
    /// Confirms a user's email address using a token.
    /// </summary>
    /// <param name="email">The user's email.</param>
    /// <param name="token">The email confirmation token.</param>
    /// <returns>An AuthResult indicating whether the operation succeeded.</returns>
    public async Task<AuthResult> ConfirmEmailAsync(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return new AuthResult
            {
                Succeeded = false,
                Error = "Confirmation failed due to an unknown error."
            };
        }

        var decoded = Uri.UnescapeDataString(token);
        var result = await _userManager.ConfirmEmailAsync(user, decoded);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return new AuthResult
            {
                Succeeded = false,
                Error = errors
            };
        }

        return new AuthResult
        {
            Succeeded = true,
            UserId = user.Id
        };
    }

    /// <summary>
    /// Generates a password reset token and sends a password reset link to the specified email.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SendPasswordResetEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return;
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encoded = Uri.EscapeDataString(token);

        var url = $"{_jwtSettings.ResetPasswordUrl}?email={Uri.EscapeDataString(email)}&token={encoded}";

        await _emailService.SendEmailAsync(email, "Reset your password",
            $"Click the link to reset your password: {url}");
    }

    /// <summary>
    /// Resets a user's password.
    /// </summary>
    /// <param name="email">The user's email.</param>
    /// <param name="token">The password reset token.</param>
    /// <param name="newPassword">The new password.</param>
    /// <returns>An AuthResult indicating whether the operation succeeded.</returns>
    public async Task<AuthResult> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return new AuthResult
            {
                Succeeded = false,
                Error = "Reset password failed due to an unknown error."
            };
        }

        var decoded = Uri.UnescapeDataString(token);
        var result = await _userManager.ResetPasswordAsync(user, decoded, newPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return new AuthResult
            {
                Succeeded = false,
                Error = errors
            };
        }

        return new AuthResult
        {
            Succeeded = true,
            UserId = user.Id
        };
    }

    public async Task<string?> GetUserEmailByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user?.Email;
    }

    public async Task<AuthenticatedUser?> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            return new AuthenticatedUser { FirstName = user.FirstName, LastName = user.LastName, Email = user.Email! };
        }
        return null;
    }

    private async Task<string> GenerateJwtAsync(User user)
    {
        var authUser = _mapper.Map<AuthenticatedUser>(user);

        var claims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        return _jwtTokenGenerator.GenerateToken(authUser, claims, roles);
    }
}
