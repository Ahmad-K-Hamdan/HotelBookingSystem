using MediatR;

namespace HotelBookingSystem.Application.Authentication.Commands.Register;

public class RegisterCommand : IRequest<RegisterResponseDto>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime BirthDate { get; set; }
}
