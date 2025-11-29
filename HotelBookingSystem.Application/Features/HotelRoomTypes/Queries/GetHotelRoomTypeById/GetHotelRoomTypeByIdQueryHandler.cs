using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Features.HotelRoomTypes.Queries.GetHotelRoomTypeById.Dtos;
using HotelBookingSystem.Domain.Entities.Rooms;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.HotelRoomTypes.Queries.GetHotelRoomTypeById;

public class GetHotelRoomTypeByIdQueryHandler : IRequestHandler<GetHotelRoomTypeByIdQuery, HotelRoomTypeDetailsDto>
{
    private readonly IGenericRepository<HotelRoomType> _roomTypeRepository;

    public GetHotelRoomTypeByIdQueryHandler(IGenericRepository<HotelRoomType> roomTypeRepository)
    {
        _roomTypeRepository = roomTypeRepository;
    }

    public async Task<HotelRoomTypeDetailsDto> Handle(GetHotelRoomTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var roomType = await _roomTypeRepository.Query()
            .Include(rt => rt.Hotel)
                .ThenInclude(h => h.City)
            .Include(rt => rt.Rooms)
            .FirstOrDefaultAsync(rt => rt.Id == request.Id, cancellationToken);

        if (roomType is null)
        {
            throw new NotFoundException(nameof(HotelRoomType), request.Id);
        }

        return new HotelRoomTypeDetailsDto
        {
            Id = roomType.Id,
            HotelId = roomType.HotelId,
            Name = roomType.Name,
            Description = roomType.Description,
            PricePerNight = roomType.PricePerNight,
            BedsCount = roomType.BedsCount,
            MaxNumOfGuestsAdults = roomType.MaxNumOfGuestsAdults,
            MaxNumOfGuestsChildren = roomType.MaxNumOfGuestsChildren,
            Hotel = new HotelForRoomTypeDto
            {
                Id = roomType.Hotel.Id,
                HotelName = roomType.Hotel.HotelName,
                CityName = roomType.Hotel.City.CityName,
                CountryName = roomType.Hotel.City.CountryName,
                StarRating = roomType.Hotel.StarRating
            },
            Rooms = roomType.Rooms
                .OrderBy(r => r.RoomNumber)
                .Select(r => new RoomInRoomTypeDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    IsAvailable = r.IsAvailable,
                    CreatedAt = r.CreatedAt
                })
                .ToList()
        };
    }
}
