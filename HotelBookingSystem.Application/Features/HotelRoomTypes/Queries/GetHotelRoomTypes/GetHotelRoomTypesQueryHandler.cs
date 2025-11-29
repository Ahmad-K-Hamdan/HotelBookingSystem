using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Rooms;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.HotelRoomTypes.Queries.GetHotelRoomTypes;

public class GetHotelRoomTypesQueryHandler : IRequestHandler<GetHotelRoomTypesQuery, List<HotelRoomTypeListDto>>
{
    private readonly IGenericRepository<HotelRoomType> _roomTypeRepository;

    public GetHotelRoomTypesQueryHandler(IGenericRepository<HotelRoomType> roomTypeRepository)
    {
        _roomTypeRepository = roomTypeRepository;
    }

    public async Task<List<HotelRoomTypeListDto>> Handle(GetHotelRoomTypesQuery request, CancellationToken cancellationToken)
    {
        return await _roomTypeRepository.Query()
            .Include(rt => rt.Hotel)
            .Include(rt => rt.Rooms)
            .Select(rt => new HotelRoomTypeListDto
            {
                Id = rt.Id,
                HotelId = rt.HotelId,
                HotelName = rt.Hotel.HotelName,
                Name = rt.Name,
                PricePerNight = rt.PricePerNight,
                BedsCount = rt.BedsCount,
                MaxNumOfGuestsAdults = rt.MaxNumOfGuestsAdults,
                MaxNumOfGuestsChildren = rt.MaxNumOfGuestsChildren,
                RoomsCount = rt.Rooms.Count
            })
            .ToListAsync(cancellationToken);
    }
}
