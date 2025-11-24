namespace HotelBookingSystem.Application.Features.Amenities.Queries.GetAmenities;

public class AmenityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
