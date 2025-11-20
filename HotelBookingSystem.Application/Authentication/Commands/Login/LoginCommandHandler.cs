using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using MediatR;

namespace HotelBookingSystem.Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _identityService.LoginAsync(request.Email, request.Password);

        if (!result.Succeeded)
        {
            throw new IdentityException(result!.Error ?? "An issue has occurred.");
        }

        return new LoginResponseDto
        {
            Token = result.Token
        };
    }
}
