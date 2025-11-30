using MediatR;

namespace HotelBookingSystem.Application.Features.HotelRooms.Commands.DeleteHotelRoom;

public record DeleteHotelRoomCommand(Guid Id) : IRequest<Unit>;
