using MediatR;

namespace HotelBookingSystem.Application.Features.HotelRooms.Queries.GetHotelRooms;

public record GetHotelRoomsQuery() : IRequest<List<HotelRoomListDto>>;
