using HotelBookingSystem.Domain.Entities.Hotels;

namespace HotelBookingSystem.Domain.Entities.Amenities;

public class Amenity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<HotelAmenity> HotelAmenities { get; set; } = new List<HotelAmenity>();
}
