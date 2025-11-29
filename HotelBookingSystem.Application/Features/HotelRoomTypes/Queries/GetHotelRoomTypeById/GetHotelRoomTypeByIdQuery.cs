using HotelBookingSystem.Application.Features.HotelRoomTypes.Queries.GetHotelRoomTypeById.Dtos;
using MediatR;

namespace HotelBookingSystem.Application.Features.HotelRoomTypes.Queries.GetHotelRoomTypeById;

public record GetHotelRoomTypeByIdQuery(Guid Id) : IRequest<HotelRoomTypeDetailsDto>;
