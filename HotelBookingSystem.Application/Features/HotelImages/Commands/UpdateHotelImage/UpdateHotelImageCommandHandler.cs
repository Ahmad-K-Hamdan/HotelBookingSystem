using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Hotels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.HotelImages.Commands.UpdateHotelImage;

public class UpdateHotelImageCommandHandler : IRequestHandler<UpdateHotelImageCommand, Unit>
{
    private readonly IGenericRepository<HotelImage> _imageRepository;
    private readonly IGenericRepository<Hotel> _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateHotelImageCommandHandler(
        IGenericRepository<HotelImage> imageRepository,
        IGenericRepository<Hotel> hotelRepository,
        IUnitOfWork unitOfWork)
    {
        _imageRepository = imageRepository;
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateHotelImageCommand request, CancellationToken cancellationToken)
    {
        var image = await _imageRepository.GetByIdAsync(request.Id);

        if (image is null)
        {
            throw new NotFoundException(nameof(HotelImage), request.Id);
        }

        _ = await _hotelRepository.GetByIdAsync(request.HotelId)
            ?? throw new NotFoundException(nameof(Hotel), request.HotelId);

        var mainImage = await _imageRepository.Query()
            .Where(img => img.HotelId == request.HotelId)
            .FirstOrDefaultAsync(img => img.IsMain, cancellationToken);

        if (request.IsMain && mainImage != null && request.Id != mainImage.Id)
        {
            throw new DuplicateRecordException("This room type already has a main image");
        }

        image.HotelId = request.HotelId;
        image.Url = request.Url;
        image.IsMain = request.IsMain;

        _imageRepository.Update(image);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}
