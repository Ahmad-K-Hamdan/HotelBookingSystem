namespace HotelBookingSystem.Domain.Entities;

public class HotelGroup
{
    public Guid Id { get; set; }
    public string GroupName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
}
