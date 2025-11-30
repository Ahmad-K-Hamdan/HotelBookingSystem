using MediatR;

namespace HotelBookingSystem.Application.Features.RoomTypeImages.Queries.GetRoomTypeImages;

public record GetRoomTypeImagesQuery() : IRequest<List<RoomTypeImageListDto>>;
