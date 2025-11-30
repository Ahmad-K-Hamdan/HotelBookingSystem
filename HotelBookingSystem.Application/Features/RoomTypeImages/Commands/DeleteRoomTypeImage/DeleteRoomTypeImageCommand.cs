using MediatR;

namespace HotelBookingSystem.Application.Features.RoomTypeImages.Commands.DeleteRoomTypeImage;

public record DeleteRoomTypeImageCommand(Guid Id) : IRequest<Unit>;
