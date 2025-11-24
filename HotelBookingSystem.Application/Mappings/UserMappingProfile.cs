using AutoMapper;
using HotelBookingSystem.Application.Common.Models;
using HotelBookingSystem.Application.Features.Authentication.Commands.Register;

namespace HotelBookingSystem.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<RegisterCommand, RegisterUserModel>();
    }
}
