using MediatR;

namespace HotelBookingSystem.Application.Features.Guests.Queries.GetGuests;

public record GetGuestsQuery() : IRequest<List<GuestListDto>>;
