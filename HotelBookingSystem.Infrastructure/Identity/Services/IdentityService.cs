using AutoMapper;
using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Common.Models;
using HotelBookingSystem.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelBookingSystem.Infrastructure.Identity.Services;
public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;

    public IdentityService(UserManager<User> userManager,
        IJwtTokenGenerator jwtTokenGenerator,
        IMapper mapper)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
    }

    public Task ConfirmEmailAsync(string email, string token)
    {
        throw new NotImplementedException();
    }

    public Task<AuthResult> LoginAsync(string email, string password)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Registers a new user, assigns the User role, sends a confirmation email,
    /// and returns a JWT authentication result on success.
    /// </summary>
    public async Task<AuthResult> RegisterAsync(RegisterUserModel registerUserModel)
    {
        var existing = await _userManager.FindByEmailAsync(registerUserModel.Email);

        if (existing != null)
        {
            throw new IdentityException("Email already exists.");
        }

        var user = _mapper.Map<User>(registerUserModel);

        var result = await _userManager.CreateAsync(user, registerUserModel.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new IdentityException(errors);
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

    public Task ResetPasswordAsync(string email, string token, string newPassword)
    {
        throw new NotImplementedException();
    }

    public Task SendConfirmationEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

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
