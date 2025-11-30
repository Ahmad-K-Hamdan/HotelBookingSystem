using MediatR;

namespace HotelBookingSystem.Application.Features.RoomTypeImages.Commands.UpdateRoomTypeImage;

public record UpdateRoomTypeImageCommand(Guid Id, Guid HotelRoomTypeId, string Url, bool IsMain) : IRequest<Unit>;
