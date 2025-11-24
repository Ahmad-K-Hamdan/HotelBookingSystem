using MediatR;

namespace HotelBookingSystem.Application.Cities.Queries.GetCityById;

public record GetCityByIdQuery(Guid Id) : IRequest<CityDetailsDto>;
