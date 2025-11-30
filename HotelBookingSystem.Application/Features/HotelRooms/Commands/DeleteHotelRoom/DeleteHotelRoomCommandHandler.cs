using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Rooms;
using MediatR;

namespace HotelBookingSystem.Application.Features.HotelRooms.Commands.DeleteHotelRoom;

public class DeleteHotelRoomCommandHandler : IRequestHandler<DeleteHotelRoomCommand, Unit>
{
    private readonly IGenericRepository<HotelRoom> _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteHotelRoomCommandHandler(IGenericRepository<HotelRoom> roomRepository, IUnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteHotelRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(request.Id);

        if (room is null)
        {
            throw new NotFoundException(nameof(HotelRoom), request.Id);
        }

        _roomRepository.Delete(room);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}
