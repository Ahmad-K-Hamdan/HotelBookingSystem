using MediatR;

namespace HotelBookingSystem.Application.Features.HotelRoomTypes.Commands.UpdateHotelRoomType;

public record UpdateHotelRoomTypeCommand(Guid Id, UpdateHotelRoomTypeDto HotelRoomType) : IRequest<Unit>;
