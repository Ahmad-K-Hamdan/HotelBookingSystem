namespace HotelBookingSystem.Application.Cities.Queries.GetCities;

public class CityDto
{
    public Guid Id { get; set; }
    public string CityName { get; set; } = null!;
    public string CountryName { get; set; } = null!;
    public string? Description { get; set; }
}
