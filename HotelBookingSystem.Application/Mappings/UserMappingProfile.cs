using AutoMapper;
using HotelBookingSystem.Application.Authentication.Commands.Register;
using HotelBookingSystem.Application.Common.Models;

namespace HotelBookingSystem.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<RegisterCommand, RegisterUserModel>();
    }
}
