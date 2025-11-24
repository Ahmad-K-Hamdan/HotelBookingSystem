using HotelBookingSystem.Application.Common.Interfaces;
using MediatR;

namespace HotelBookingSystem.Application.Features.Authentication.Commands.EmailConfirmation;

public class SendConfirmationEmailCommandHandler : IRequestHandler<SendConfirmationEmailCommand>
{
    private readonly IIdentityService _identityService;

    public SendConfirmationEmailCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task Handle(SendConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        await _identityService.SendConfirmationEmailAsync(request.Email);
    }
}
