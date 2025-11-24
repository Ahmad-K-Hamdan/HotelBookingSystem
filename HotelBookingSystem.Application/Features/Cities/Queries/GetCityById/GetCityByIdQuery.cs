using MediatR;

namespace HotelBookingSystem.Application.Features.Cities.Queries.GetCityById;

public record GetCityByIdQuery(Guid Id) : IRequest<CityDetailsDto>;
