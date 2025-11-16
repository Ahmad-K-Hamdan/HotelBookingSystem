using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Common.Models;

namespace HotelBookingSystem.Infrastructure.Identity.Services;
public class IdentityService : IIdentityService
{
    public Task ConfirmEmailAsync(string email, string token)
    {
        throw new NotImplementedException();
    }

    public Task<AuthResult> LoginAsync(string email, string password)
    {
        throw new NotImplementedException();
    }

    public Task<AuthResult> RegisterAsync(string firstName, string lastName, string email, string password, DateTime? birthDate)
    {
        throw new NotImplementedException();
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
}
