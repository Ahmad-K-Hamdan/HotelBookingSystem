namespace HotelBookingSystem.Application.Cities.Queries.GetCityById;

public class CityDetailsDto
{
    public Guid Id { get; set; }
    public string CityName { get; set; } = null!;
    public string CountryName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
