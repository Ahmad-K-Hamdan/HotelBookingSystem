using MediatR;

namespace HotelBookingSystem.Application.Features.Cities.Commands.CreateCity;

public record CreateCityCommand(string CityName, string CountryName, string? Description) : IRequest<Guid>;
