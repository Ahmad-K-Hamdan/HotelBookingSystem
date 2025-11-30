using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Rooms;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.RoomTypeImages.Queries.GetRoomTypeImages;

public class GetRoomTypeImagesQueryHandler : IRequestHandler<GetRoomTypeImagesQuery, List<RoomTypeImageListDto>>
{
    private readonly IGenericRepository<RoomTypeImage> _imageRepository;

    public GetRoomTypeImagesQueryHandler(IGenericRepository<RoomTypeImage> imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public async Task<List<RoomTypeImageListDto>> Handle(GetRoomTypeImagesQuery request, CancellationToken cancellationToken)
    {
        return await _imageRepository.Query()
            .Include(i => i.HotelRoomType)
                .ThenInclude(rt => rt.Hotel)
            .Select(i => new RoomTypeImageListDto
            {
                Id = i.Id,
                HotelRoomTypeId = i.HotelRoomTypeId,
                RoomTypeName = i.HotelRoomType.Name,
                HotelId = i.HotelRoomType.HotelId,
                HotelName = i.HotelRoomType.Hotel.HotelName,
                Url = i.Url,
                IsMain = i.IsMain
            })
            .OrderBy(i => i.HotelName)
            .ThenBy(i => i.RoomTypeName)
            .ThenByDescending(i => i.IsMain)
            .ToListAsync(cancellationToken);
    }
}
