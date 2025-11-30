using MediatR;

namespace HotelBookingSystem.Application.Features.HotelRooms.Commands.UpdateHotelRoom;

public record UpdateHotelRoomCommand(Guid Id, Guid HotelRoomTypeId, int RoomNumber, bool IsAvailable) : IRequest<Unit>;
