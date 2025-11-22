using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Common.Models;
using MediatR;

namespace HotelBookingSystem.Application.Authentication.Commands.PasswordReset;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, AuthResult>
{
    private readonly IIdentityService _identityService;

    public ResetPasswordCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<AuthResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.ResetPasswordAsync(
            request.Email, request.Token, request.NewPassword);

        if (!result.Succeeded)
        {
            throw new IdentityException(result.Error ?? "Password reset failed.");
        }

        return result;
    }
}
