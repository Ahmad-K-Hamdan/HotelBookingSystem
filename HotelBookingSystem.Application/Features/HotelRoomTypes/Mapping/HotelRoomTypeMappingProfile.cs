using AutoMapper;
using HotelBookingSystem.Application.Features.HotelRoomTypes.Commands.CreateHotelRoomType;
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
