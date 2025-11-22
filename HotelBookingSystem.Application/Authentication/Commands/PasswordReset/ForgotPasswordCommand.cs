using MediatR;

namespace HotelBookingSystem.Application.Authentication.Commands.PasswordReset;

public class ForgotPasswordCommand : IRequest
{
    public string Email { get; set; } = null!;
}
