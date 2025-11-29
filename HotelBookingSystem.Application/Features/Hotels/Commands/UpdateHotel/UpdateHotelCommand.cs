using MediatR;

namespace HotelBookingSystem.Application.Features.Hotels.Commands.UpdateHotel;

public record UpdateHotelCommand(Guid Id, UpdateHotelDto Hotel) : IRequest<Unit>;
