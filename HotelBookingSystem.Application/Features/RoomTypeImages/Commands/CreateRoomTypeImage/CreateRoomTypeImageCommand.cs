using MediatR;

namespace HotelBookingSystem.Application.Features.RoomTypeImages.Commands.CreateRoomTypeImage;

public record CreateRoomTypeImageCommand(Guid HotelRoomTypeId, string Url, bool IsMain) : IRequest<Guid>;
