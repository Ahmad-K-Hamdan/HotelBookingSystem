using HotelBookingSystem.Application.Features.HotelRooms.Queries.GetHotelRoomById.Dtos;
using MediatR;

namespace HotelBookingSystem.Application.Features.HotelRooms.Queries.GetHotelRoomById;

public record GetHotelRoomByIdQuery(Guid Id) : IRequest<HotelRoomDetailsDto>;
