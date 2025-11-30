using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Features.HotelRooms.Queries.GetHotelRoomById.Dtos;
using HotelBookingSystem.Domain.Entities.Rooms;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.HotelRooms.Queries.GetHotelRoomById;

public class GetHotelRoomByIdQueryHandler : IRequestHandler<GetHotelRoomByIdQuery, HotelRoomDetailsDto>
{
    private readonly IGenericRepository<HotelRoom> _roomRepository;

    public GetHotelRoomByIdQueryHandler(IGenericRepository<HotelRoom> roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<HotelRoomDetailsDto> Handle(GetHotelRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.Query()
            .Include(r => r.RoomType)
                .ThenInclude(rt => rt.Hotel)
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (room is null)
        {
            throw new NotFoundException(nameof(HotelRoom), request.Id);
        }

        return new HotelRoomDetailsDto
        {
            Id = room.Id,
            HotelRoomTypeId = room.HotelRoomTypeId,
            RoomNumber = room.RoomNumber,
            IsAvailable = room.IsAvailable,
            CreatedAt = room.CreatedAt,
            UpdatedAt = room.UpdatedAt,
            RoomType = new RoomTypeForRoomDto
            {
                Id = room.RoomType.Id,
                HotelId = room.RoomType.HotelId,
                RoomTypeName = room.RoomType.Name,
                Description = room.RoomType.Description,
                PricePerNight = room.RoomType.PricePerNight,
                BedsCount = room.RoomType.BedsCount,
                MaxNumOfGuestsAdults = room.RoomType.MaxNumOfGuestsAdults,
                MaxNumOfGuestsChildren = room.RoomType.MaxNumOfGuestsChildren,
                Images = room.RoomType.Images
                .OrderByDescending(i => i.IsMain)
                .ThenBy(i => i.Id)
                .Select(i => new RoomTypeImageDto
                {
                    Id = i.Id,
                    Url = i.Url,
                    IsMain = i.IsMain
                })
                .ToList()
            },
        };
    }
}
