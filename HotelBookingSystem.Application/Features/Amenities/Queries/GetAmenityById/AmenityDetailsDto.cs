namespace HotelBookingSystem.Application.Features.Amenities.Queries.GetAmenityById;

public class AmenityDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
