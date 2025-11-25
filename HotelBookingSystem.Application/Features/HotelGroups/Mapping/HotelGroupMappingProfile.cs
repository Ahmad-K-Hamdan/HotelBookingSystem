using AutoMapper;
using HotelBookingSystem.Application.Features.HotelGroups.Queries.GetHotelGroupById;
using HotelBookingSystem.Domain.Entities.Hotels;

namespace HotelBookingSystem.Application.Features.HotelGroups.Mapping;

public class HotelGroupMappingProfile : Profile
{
    public HotelGroupMappingProfile()
    {
        CreateMap<HotelGroup, HotelGroupDetailsDto>();
    }
}
