using MediatR;

namespace HotelBookingSystem.Application.Features.HotelRoomTypes.Queries.GetHotelRoomTypes;

public record GetHotelRoomTypesQuery() : IRequest<List<HotelRoomTypeListDto>>;
