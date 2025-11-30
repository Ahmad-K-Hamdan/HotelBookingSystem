using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Rooms;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.HotelRooms.Queries.GetHotelRooms;

public class GetHotelRoomsQueryHandler : IRequestHandler<GetHotelRoomsQuery, List<HotelRoomListDto>>
{
    private readonly IGenericRepository<HotelRoom> _roomRepository;

    public GetHotelRoomsQueryHandler(IGenericRepository<HotelRoom> roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<List<HotelRoomListDto>> Handle(GetHotelRoomsQuery request, CancellationToken cancellationToken)
    {
        return await _roomRepository.Query()
            .Include(r => r.RoomType)
                .ThenInclude(rt => rt.Hotel)
            .Select(r => new HotelRoomListDto
            {
                Id = r.Id,
                HotelRoomTypeId = r.HotelRoomTypeId,
                RoomNumber = r.RoomNumber,
                IsAvailable = r.IsAvailable,
                HotelId = r.RoomType.HotelId,
                HotelName = r.RoomType.Hotel.HotelName,
                RoomTypeName = r.RoomType.Name
            })
            .OrderBy(r => r.HotelName)
            .ThenBy(r => r.RoomTypeName)
            .ThenBy(r => r.RoomNumber)
            .ToListAsync(cancellationToken);
    }
}
