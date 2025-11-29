using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Rooms;
using MediatR;

namespace HotelBookingSystem.Application.Features.HotelRoomTypes.Commands.DeleteHotelRoomType;

public class DeleteHotelRoomTypeCommandHandler : IRequestHandler<DeleteHotelRoomTypeCommand, Unit>
{
    private readonly IGenericRepository<HotelRoomType> _roomTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteHotelRoomTypeCommandHandler(IGenericRepository<HotelRoomType> roomTypeRepository, IUnitOfWork unitOfWork)
    {
        _roomTypeRepository = roomTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteHotelRoomTypeCommand request, CancellationToken cancellationToken)
    {
        var roomType = await _roomTypeRepository.GetByIdAsync(request.Id);

        if (roomType is null)
        {
            throw new NotFoundException(nameof(HotelRoomType), request.Id);
        }

        _roomTypeRepository.Delete(roomType);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}
