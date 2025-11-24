using HotelBookingSystem.Application.Common.Models;
using MediatR;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.EmailConfirmation;

public class ConfirmEmailCommand : IRequest<AuthResult>
{
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
}
