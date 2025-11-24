using HotelBookingSystem.Application.Common.Models;
using MediatR;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.PasswordReset;

public class ResetPasswordCommand : IRequest<AuthResult>
{
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}
