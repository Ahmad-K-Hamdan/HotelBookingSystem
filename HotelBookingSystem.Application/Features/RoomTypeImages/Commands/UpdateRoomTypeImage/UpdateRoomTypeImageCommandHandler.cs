using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Rooms;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.RoomTypeImages.Commands.UpdateRoomTypeImage;

public class UpdateRoomTypeImageCommandHandler : IRequestHandler<UpdateRoomTypeImageCommand, Unit>
{
    private readonly IGenericRepository<RoomTypeImage> _imageRepository;
    private readonly IGenericRepository<HotelRoomType> _roomTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoomTypeImageCommandHandler(
        IGenericRepository<RoomTypeImage> imageRepository,
        IGenericRepository<HotelRoomType> roomTypeRepository,
        IUnitOfWork unitOfWork)
    {
        _imageRepository = imageRepository;
        _roomTypeRepository = roomTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateRoomTypeImageCommand request, CancellationToken cancellationToken)
    {
        var image = await _imageRepository.GetByIdAsync(request.Id);

        if (image is null)
        {
            throw new NotFoundException(nameof(RoomTypeImage), request.Id);
        }

        _ = await _roomTypeRepository.GetByIdAsync(request.HotelRoomTypeId)
            ?? throw new NotFoundException(nameof(HotelRoomType), request.HotelRoomTypeId);

        var mainImage = await _imageRepository.Query()
            .Where(img => img.HotelRoomTypeId == request.HotelRoomTypeId)
            .FirstOrDefaultAsync(img => img.IsMain, cancellationToken);

        if (request.IsMain && mainImage != null && request.Id != mainImage.Id)
        {
            throw new DuplicateRecordException("This room type already has a main image");
        }

        image.HotelRoomTypeId = request.HotelRoomTypeId;
        image.Url = request.Url;
        image.IsMain = request.IsMain;

        _imageRepository.Update(image);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}
