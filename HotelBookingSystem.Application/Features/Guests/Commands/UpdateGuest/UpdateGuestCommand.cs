using MediatR;

namespace HotelBookingSystem.Application.Features.Guests.Commands.UpdateGuest;

public record UpdateGuestCommand(Guid Id, string PassportNumber, string HomeCountry) : IRequest<Unit>;
