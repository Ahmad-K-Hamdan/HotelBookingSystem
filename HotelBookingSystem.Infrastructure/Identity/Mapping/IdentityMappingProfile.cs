using AutoMapper;
using HotelBookingSystem.Application.Common.Models;
using HotelBookingSystem.Infrastructure.Identity.Models;

namespace HotelBookingSystem.Infrastructure.Identity.Mapping;

public class IdentityMappingProfile : Profile
{
    public IdentityMappingProfile()
    {
        CreateMap<RegisterUserModel, User>()
            .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.EmailConfirmed,
                opt => opt.MapFrom(_ => false));

        CreateMap<User, AuthenticatedUser>()
            .ForMember(dest => dest.UserId,
                opt => opt.MapFrom(src => src.Id));
    }
}
