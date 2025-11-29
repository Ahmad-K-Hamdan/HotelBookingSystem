using MediatR;

namespace HotelBookingSystem.Application.Features.Guests.Commands.CreateGuest;

public record CreateGuestCommand(string PassportNumber, string HomeCountry) : IRequest<Guid>;
