using HotelBookingSystem.Application.Features.Guests.Queries.GetGuestById.Dtos;
using MediatR;

namespace HotelBookingSystem.Application.Features.Guests.Queries.GetGuestById;

public record GetGuestByIdQuery(Guid Id) : IRequest<GuestDetailsDto>;
