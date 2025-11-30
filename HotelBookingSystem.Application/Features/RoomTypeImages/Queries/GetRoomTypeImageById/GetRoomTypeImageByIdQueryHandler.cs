using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Rooms;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.RoomTypeImages.Queries.GetRoomTypeImageById;

public class GetRoomTypeImageByIdQueryHandler : IRequestHandler<GetRoomTypeImageByIdQuery, RoomTypeImageDetailsDto>
{
    private readonly IGenericRepository<RoomTypeImage> _imageRepository;

    public GetRoomTypeImageByIdQueryHandler(IGenericRepository<RoomTypeImage> imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public async Task<RoomTypeImageDetailsDto> Handle(GetRoomTypeImageByIdQuery request, CancellationToken cancellationToken)
    {
        var image = await _imageRepository.Query()
            .Include(i => i.HotelRoomType)
                .ThenInclude(rt => rt.Hotel)
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (image is null)
        {
            throw new NotFoundException(nameof(RoomTypeImage), request.Id);
        }

        var roomType = image.HotelRoomType;

        return new RoomTypeImageDetailsDto
        {
            Id = image.Id,
            HotelRoomTypeId = image.HotelRoomTypeId,
            RoomTypeName = roomType.Name,
            HotelId = roomType.HotelId,
            HotelName = roomType.Hotel.HotelName,
            PricePerNight = roomType.PricePerNight,
            BedsCount = roomType.BedsCount,
            MaxNumOfGuestsAdults = roomType.MaxNumOfGuestsAdults,
            MaxNumOfGuestsChildren = roomType.MaxNumOfGuestsChildren,
            Url = image.Url,
            IsMain = image.IsMain
        };
    }
}
