using MediatR;

namespace HotelBookingSystem.Application.Cities.Commands.DeleteCity;

public record DeleteCityCommand(Guid Id) : IRequest<Unit>;
