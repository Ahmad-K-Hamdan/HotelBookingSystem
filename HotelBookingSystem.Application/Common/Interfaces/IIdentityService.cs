using HotelBookingSystem.Application.Common.Models;

namespace HotelBookingSystem.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<AuthResult> RegisterAsync(RegisterUserModel registerUserModel);
    Task<AuthResult> LoginAsync(string email, string password);

    Task SendConfirmationEmailAsync(string email);
    Task<AuthResult> ConfirmEmailAsync(string email, string token);

    Task SendPasswordResetEmailAsync(string email);
    Task<AuthResult> ResetPasswordAsync(string email, string token, string newPassword);

    Task<string?> GetUserEmailByIdAsync(string userId);
    Task<AuthenticatedUser?> GetUserByIdAsync(string userId);
}
