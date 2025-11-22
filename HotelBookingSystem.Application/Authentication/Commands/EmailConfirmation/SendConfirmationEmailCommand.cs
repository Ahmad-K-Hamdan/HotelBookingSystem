using MediatR;

namespace HotelBookingSystem.Application.Authentication.Commands.EmailConfirmation;

public class SendConfirmationEmailCommand : IRequest
{
    public string Email { get; set; } = null!;
}