using MediatR;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.Register;

public class RegisterCommand : IRequest<RegisterResponseDto>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateOnly BirthDate { get; set; }
}
