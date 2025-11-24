using MediatR;

namespace HotelBookingSystem.Application.Cities.Commands.CreateCity;

public record CreateCityCommand(string CityName, string CountryName, string? Description) : IRequest<Guid>;
