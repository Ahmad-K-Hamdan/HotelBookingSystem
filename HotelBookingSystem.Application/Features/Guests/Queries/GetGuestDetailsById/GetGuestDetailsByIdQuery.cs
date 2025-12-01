using HotelBookingSystem.Application.Features.Guests.Queries.GetGuestDetailsById.Dtos;
using MediatR;

namespace HotelBookingSystem.Application.Features.Guests.Queries.GetGuestDetailsById;

public record GetGuestDetailsByIdQuery(Guid Id) : IRequest<GuestDetailsDto>;
