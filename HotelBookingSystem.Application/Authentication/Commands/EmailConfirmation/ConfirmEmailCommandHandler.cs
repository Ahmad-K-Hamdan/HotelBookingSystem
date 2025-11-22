using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Common.Models;
using MediatR;

namespace HotelBookingSystem.Application.Authentication.Commands.EmailConfirmation;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, AuthResult>
{
    private readonly IIdentityService _identityService;

    public ConfirmEmailCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<AuthResult> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.ConfirmEmailAsync(request.Email, request.Token);

        if (!result.Succeeded)
        {
            throw new IdentityException(result.Error ?? "Email confirmation failed.");
        }

        return result;
    }
}
