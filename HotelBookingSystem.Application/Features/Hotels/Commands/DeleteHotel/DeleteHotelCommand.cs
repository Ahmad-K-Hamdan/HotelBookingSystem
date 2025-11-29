using MediatR;

namespace HotelBookingSystem.Application.Features.Hotels.Commands.DeleteHotel;

public record DeleteHotelCommand(Guid Id) : IRequest<Unit>;