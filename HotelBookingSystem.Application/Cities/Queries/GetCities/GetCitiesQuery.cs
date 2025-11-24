using MediatR;

namespace HotelBookingSystem.Application.Cities.Queries.GetCities;

public record GetCitiesQuery() : IRequest<List<CityDto>>;
