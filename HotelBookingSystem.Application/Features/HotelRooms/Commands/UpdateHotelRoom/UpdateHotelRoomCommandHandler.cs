using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Rooms;
using MediatR;

namespace HotelBookingSystem.Application.Features.HotelRooms.Commands.UpdateHotelRoom;

public class UpdateHotelRoomCommandHandler : IRequestHandler<UpdateHotelRoomCommand, Unit>
{
    private readonly IGenericRepository<HotelRoom> _roomRepository;
    private readonly IGenericRepository<HotelRoomType> _roomTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateHotelRoomCommandHandler(
        IGenericRepository<HotelRoom> roomRepository,
        IGenericRepository<HotelRoomType> roomTypeRepository,
        IUnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _roomTypeRepository = roomTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateHotelRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(request.Id);

        if (room is null)
        {
            throw new NotFoundException(nameof(HotelRoom), request.Id);
        }

        _ = await _roomTypeRepository.GetByIdAsync(request.HotelRoomTypeId)
            ?? throw new NotFoundException(nameof(HotelRoomType), request.HotelRoomTypeId);

        room.HotelRoomTypeId = request.HotelRoomTypeId;
        room.RoomNumber = request.RoomNumber;
        room.IsAvailable = request.IsAvailable;
        room.UpdatedAt = DateTime.UtcNow;

        _roomRepository.Update(room);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}
