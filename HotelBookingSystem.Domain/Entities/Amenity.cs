namespace HotelBookingSystem.Domain.Entities;

public class Amenity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<HotelAmenity> HotelAmenities { get; set; } = new List<HotelAmenity>();
}
