namespace HotelBookingSystem.Domain.Entities;

public class HotelAmenity
{
    public Guid HotelId { get; set; }
    public Guid AmenityId { get; set; }

    public Hotel Hotel { get; set; } = null!;
    public Amenity Amenity { get; set; } = null!;
}
