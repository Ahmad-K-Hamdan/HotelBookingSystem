using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Hotels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.HotelImages.Queries.GetHotelImageById;

public class GetHotelImageByIdQueryHandler : IRequestHandler<GetHotelImageByIdQuery, HotelImageDetailsDto>
{
    private readonly IGenericRepository<HotelImage> _imageRepository;

    public GetHotelImageByIdQueryHandler(IGenericRepository<HotelImage> imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public async Task<HotelImageDetailsDto> Handle(GetHotelImageByIdQuery request, CancellationToken cancellationToken)
    {
        var image = await _imageRepository.Query()
            .Include(i => i.Hotel)
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (image is null)
        {
            throw new NotFoundException(nameof(HotelImage), request.Id);
        }

        return new HotelImageDetailsDto
        {
            Id = image.Id,
            HotelId = image.HotelId,
            HotelName = image.Hotel.HotelName,
            HotelAddress = image.Hotel.HotelAddress,
            Url = image.Url,
            IsMain = image.IsMain
        };
    }
}
