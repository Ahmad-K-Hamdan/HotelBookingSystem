namespace HotelBookingSystem.Application.Features.Cities.Queries.GetTrendingCities;

public class TrendingCityDto
{
    public Guid CityId { get; set; }
    public string CityName { get; set; } = null!;
    public string CountryName { get; set; } = null!;
    public int VisitCount { get; set; }
}
