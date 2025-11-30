using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Rooms;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.RoomTypeImages.Commands.CreateRoomTypeImage;

public class CreateRoomTypeImageCommandHandler : IRequestHandler<CreateRoomTypeImageCommand, Guid>
{
    private readonly IGenericRepository<RoomTypeImage> _imageRepository;
    private readonly IGenericRepository<HotelRoomType> _roomTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoomTypeImageCommandHandler(
        IGenericRepository<RoomTypeImage> imageRepository,
        IGenericRepository<HotelRoomType> roomTypeRepository,
        IUnitOfWork unitOfWork)
    {
        _imageRepository = imageRepository;
        _roomTypeRepository = roomTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateRoomTypeImageCommand request, CancellationToken cancellationToken)
    {
        _ = await _roomTypeRepository.GetByIdAsync(request.HotelRoomTypeId)
            ?? throw new NotFoundException(nameof(HotelRoomType), request.HotelRoomTypeId);

        var mainImageExists = await _imageRepository.Query()
            .Where(img => img.HotelRoomTypeId == request.HotelRoomTypeId)
            .AnyAsync(img => img.IsMain, cancellationToken);

        if (request.IsMain && mainImageExists)
        {
            throw new DuplicateRecordException("This room type already has a main image");
        }

        var image = new RoomTypeImage
        {
            Id = Guid.NewGuid(),
            HotelRoomTypeId = request.HotelRoomTypeId,
            Url = request.Url,
            IsMain = request.IsMain
        };

        await _imageRepository.AddAsync(image);
        await _unitOfWork.SaveChangesAsync();
        return image.Id;
    }
}
