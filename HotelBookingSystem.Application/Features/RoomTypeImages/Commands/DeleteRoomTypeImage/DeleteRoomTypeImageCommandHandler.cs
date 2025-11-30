using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Rooms;
using MediatR;

namespace HotelBookingSystem.Application.Features.RoomTypeImages.Commands.DeleteRoomTypeImage;

public class DeleteRoomTypeImageCommandHandler : IRequestHandler<DeleteRoomTypeImageCommand, Unit>
{
    private readonly IGenericRepository<RoomTypeImage> _imageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoomTypeImageCommandHandler(
        IGenericRepository<RoomTypeImage> imageRepository,
        IUnitOfWork unitOfWork)
    {
        _imageRepository = imageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteRoomTypeImageCommand request, CancellationToken cancellationToken)
    {
        var image = await _imageRepository.GetByIdAsync(request.Id);

        if (image is null)
        {
            throw new NotFoundException(nameof(RoomTypeImage), request.Id);
        }

        _imageRepository.Delete(image);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}
