using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Hotels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.HotelImages.Queries.GetHotelImages;

public class GetHotelImagesQueryHandler : IRequestHandler<GetHotelImagesQuery, List<HotelImageListDto>>
{
    private readonly IGenericRepository<HotelImage> _imageRepository;

    public GetHotelImagesQueryHandler(IGenericRepository<HotelImage> imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public async Task<List<HotelImageListDto>> Handle(GetHotelImagesQuery request, CancellationToken cancellationToken)
    {
        return await _imageRepository.Query()
            .Include(i => i.Hotel)
            .Select(i => new HotelImageListDto
            {
                Id = i.Id,
                HotelId = i.HotelId,
                HotelName = i.Hotel.HotelName,
                Url = i.Url,
                IsMain = i.IsMain
            })
            .OrderBy(i => i.HotelName)
            .ThenByDescending(i => i.IsMain)
            .ThenBy(i => i.Id)
            .ToListAsync(cancellationToken);
    }
}
