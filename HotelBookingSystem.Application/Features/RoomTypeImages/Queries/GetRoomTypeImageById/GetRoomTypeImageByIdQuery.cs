using MediatR;

namespace HotelBookingSystem.Application.Features.RoomTypeImages.Queries.GetRoomTypeImageById;

public record GetRoomTypeImageByIdQuery(Guid Id) : IRequest<RoomTypeImageDetailsDto>;
