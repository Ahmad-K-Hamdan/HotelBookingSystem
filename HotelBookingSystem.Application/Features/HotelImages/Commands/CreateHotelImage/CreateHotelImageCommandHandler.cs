using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Hotels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.HotelImages.Commands.CreateHotelImage;

public class CreateHotelImageCommandHandler : IRequestHandler<CreateHotelImageCommand, Guid>
{
    private readonly IGenericRepository<HotelImage> _imageRepository;
    private readonly IGenericRepository<Hotel> _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHotelImageCommandHandler(
        IGenericRepository<HotelImage> imageRepository,
        IGenericRepository<Hotel> hotelRepository,
        IUnitOfWork unitOfWork)
    {
        _imageRepository = imageRepository;
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateHotelImageCommand request, CancellationToken cancellationToken)
    {
        _ = await _hotelRepository.GetByIdAsync(request.HotelId)
            ?? throw new NotFoundException(nameof(Hotel), request.HotelId);

        var mainImageExists = await _imageRepository.Query()
            .Where(img => img.HotelId == request.HotelId)
            .AnyAsync(img => img.IsMain, cancellationToken);

        if (request.IsMain && mainImageExists)
        {
            throw new DuplicateRecordException("This hotel already has a main image");
        }

        var image = new HotelImage
        {
            Id = Guid.NewGuid(),
            HotelId = request.HotelId,
            Url = request.Url,
            IsMain = request.IsMain
        };

        await _imageRepository.AddAsync(image);
        await _unitOfWork.SaveChangesAsync();
        return image.Id;
    }
}
