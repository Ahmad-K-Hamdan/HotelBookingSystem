using HotelBookingSystem.Application.Common.Models;

namespace HotelBookingSystem.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<AuthResult> RegisterAsync(RegisterUserModel registerUserModel);
    Task<AuthResult> LoginAsync(string email, string password);

    Task SendConfirmationEmailAsync(string email);
    Task ConfirmEmailAsync(string email, string token);

    Task SendPasswordResetEmailAsync(string email);
    Task ResetPasswordAsync(string email, string token, string newPassword);
}