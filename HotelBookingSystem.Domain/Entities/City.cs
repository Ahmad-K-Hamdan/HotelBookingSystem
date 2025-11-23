namespace HotelBookingSystem.Domain.Entities;

public class City
{
    public Guid Id { get; set; }
    public string CityName { get; set; } = null!;
    public string CountryName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
}
