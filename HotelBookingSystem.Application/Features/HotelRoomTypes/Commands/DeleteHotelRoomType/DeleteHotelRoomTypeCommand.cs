using MediatR;

namespace HotelBookingSystem.Application.Features.HotelRoomTypes.Commands.DeleteHotelRoomType;

public record DeleteHotelRoomTypeCommand(Guid Id) : IRequest<Unit>;
