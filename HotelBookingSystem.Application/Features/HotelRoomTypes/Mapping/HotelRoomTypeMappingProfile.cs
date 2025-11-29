using AutoMapper;
using HotelBookingSystem.Application.Features.HotelRoomTypes.Commands.CreateHotelRoomType;
using HotelBookingSystem.Application.Features.Hotels.Commands.CreateHotel;
using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels.Dtos;
using HotelBookingSystem.Domain.Entities.Hotels;
using HotelBookingSystem.Domain.Entities.Rooms;

namespace HotelBookingSystem.Application.Features.HotelRoomTypes.Mapping;

public class HotelRoomTypeMappingProfile : Profile
{
    public HotelRoomTypeMappingProfile()
    {
        CreateMap<CreateHotelRoomTypeCommand, HotelRoomType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
