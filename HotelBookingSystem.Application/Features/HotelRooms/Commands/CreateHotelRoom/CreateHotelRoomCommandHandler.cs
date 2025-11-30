using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Rooms;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.HotelRooms.Commands.CreateHotelRoom;

public class CreateHotelRoomCommandHandler : IRequestHandler<CreateHotelRoomCommand, Guid>
{
    private readonly IGenericRepository<HotelRoom> _roomRepository;
    private readonly IGenericRepository<HotelRoomType> _roomTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHotelRoomCommandHandler(
        IGenericRepository<HotelRoom> roomRepository,
        IGenericRepository<HotelRoomType> roomTypeRepository,
        IUnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _roomTypeRepository = roomTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateHotelRoomCommand request, CancellationToken cancellationToken)
    {
        var roomType = await _roomTypeRepository.GetByIdAsync(request.HotelRoomTypeId)
            ?? throw new NotFoundException(nameof(HotelRoomType), request.HotelRoomTypeId);

        var existsInRoomType = await _roomRepository.Query()
            .AnyAsync(r => r.HotelRoomTypeId == request.HotelRoomTypeId && r.RoomNumber == request.RoomNumber, cancellationToken);

        if (existsInRoomType)
        {
            throw new DuplicateRecordException($"Room number {request.RoomNumber} already exists for this room type.");
        }

        var room = new HotelRoom
        {
            Id = Guid.NewGuid(),
            HotelRoomTypeId = request.HotelRoomTypeId,
            RoomNumber = request.RoomNumber,
            IsAvailable = request.IsAvailable,
            CreatedAt = DateTime.UtcNow
        };

        await _roomRepository.AddAsync(room);
        await _unitOfWork.SaveChangesAsync();
        return room.Id;
    }
}
