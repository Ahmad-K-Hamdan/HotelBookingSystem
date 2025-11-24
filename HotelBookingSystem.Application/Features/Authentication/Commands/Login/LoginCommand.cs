using MediatR;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.Login;

public class LoginCommand : IRequest<LoginResponseDto>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
