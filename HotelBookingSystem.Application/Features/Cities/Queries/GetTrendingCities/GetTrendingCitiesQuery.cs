using MediatR;

namespace HotelBookingSystem.Application.Features.Cities.Queries.GetTrendingCities;

public class GetTrendingCitiesQuery : IRequest<List<TrendingCityDto>>
{
    public int Limit { get; set; } = 5;
    public int? DaysBack { get; set; } 
}
