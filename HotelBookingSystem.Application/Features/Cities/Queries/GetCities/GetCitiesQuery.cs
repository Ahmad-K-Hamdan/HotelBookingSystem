using MediatR;

namespace HotelBookingSystem.Application.Features.Cities.Queries.GetCities;

public record GetCitiesQuery() : IRequest<List<CityDto>>;
