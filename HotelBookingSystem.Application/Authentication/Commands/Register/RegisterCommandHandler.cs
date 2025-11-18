using AutoMapper;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Common.Models;
using MediatR;

namespace HotelBookingSystem.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponseDto>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public RegisterCommandHandler(IIdentityService identityService,
        IMapper mapper)
    {
        _identityService = identityService;
        _mapper = mapper;
    }

    public async Task<RegisterResponseDto> Handle(RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var model = _mapper.Map<RegisterUserModel>(request);

        var result = await _identityService.RegisterAsync(model);

        if (!result.Succeeded)
        {
            throw new Exception(result.Error);
        }

        return new RegisterResponseDto
        {
            UserId = result.UserId,
            Token = result.Token
        };
    }
}