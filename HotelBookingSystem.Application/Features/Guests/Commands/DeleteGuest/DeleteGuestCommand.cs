using MediatR;

namespace HotelBookingSystem.Application.Features.Guests.Commands.DeleteGuest;

public record DeleteGuestCommand(Guid Id) : IRequest<Unit>;
