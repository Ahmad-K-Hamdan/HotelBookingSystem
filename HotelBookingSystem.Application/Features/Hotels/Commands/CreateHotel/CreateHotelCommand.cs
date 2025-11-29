using MediatR;

namespace HotelBookingSystem.Application.Features.Hotels.Commands.CreateHotel;

public record CreateHotelCommand(CreateHotelDto Hotel) : IRequest<Guid>;
