using HotelBookingSystem.Application.Common.Dtos;
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
            .Include(r => r.RoomType)
                .ThenInclude(rt => rt.Images)
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (room is null)
        {
            throw new NotFoundException(nameof(HotelRoom), request.Id);
        }

        var roomType = room.RoomType;

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
                Id = roomType.Id,
                HotelId = roomType.HotelId,
                RoomTypeName = roomType.Name,
                Description = roomType.Description,
                PricePerNight = roomType.PricePerNight,
                BedsCount = roomType.BedsCount,
                MaxNumOfGuestsAdults = roomType.MaxNumOfGuestsAdults,
                MaxNumOfGuestsChildren = roomType.MaxNumOfGuestsChildren,
                Images = roomType.Images
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
