using AutoMapper;
using HotelBookingSystem.Application.Features.Hotels.Commands.CreateHotel;
using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels.Dtos;
using HotelBookingSystem.Domain.Entities.Hotels;

namespace HotelBookingSystem.Application.Features.Hotels.Mapping;

public class HotelMappingProfile : Profile
{
    public HotelMappingProfile()
    {
        CreateMap<CreateHotelDto, Hotel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<Hotel, HotelDto>();
    }
}
