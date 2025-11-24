using MediatR;

namespace HotelBookingSystem.Application.Cities.Commands.UpdateCity;

public record UpdateCityCommand(Guid Id, string CityName, string CountryName, string? Description) : IRequest<Unit>;
