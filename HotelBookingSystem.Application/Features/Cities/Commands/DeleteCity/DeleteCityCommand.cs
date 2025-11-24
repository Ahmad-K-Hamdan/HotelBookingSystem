using MediatR;

namespace HotelBookingSystem.Application.Features.Cities.Commands.DeleteCity;

public record DeleteCityCommand(Guid Id) : IRequest<Unit>;
