using HotelBookingSystem.Application.Cities.Queries.GetCities;
using MediatR;

public record GetCitiesQuery() : IRequest<List<CityDto>>;
