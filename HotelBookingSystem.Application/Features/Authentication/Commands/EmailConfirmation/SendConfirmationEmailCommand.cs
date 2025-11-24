using MediatR;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.EmailConfirmation;

public class SendConfirmationEmailCommand : IRequest
{
    public string Email { get; set; } = null!;
}