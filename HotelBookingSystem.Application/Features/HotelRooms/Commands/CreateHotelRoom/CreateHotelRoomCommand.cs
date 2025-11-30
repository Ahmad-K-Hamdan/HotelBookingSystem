using MediatR;

namespace HotelBookingSystem.Application.Features.HotelRooms.Commands.CreateHotelRoom;

public record CreateHotelRoomCommand(Guid HotelRoomTypeId, int RoomNumber, bool IsAvailable) : IRequest<Guid>;
