using HotelBookingSystem.Application.Common.Interfaces;
using MediatR;

namespace HotelBookingSystem.Application.Authentication.Commands.PasswordReset;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
{
    private readonly IIdentityService _identityService;

    public ForgotPasswordCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        await _identityService.SendPasswordResetEmailAsync(request.Email);
    }
}
