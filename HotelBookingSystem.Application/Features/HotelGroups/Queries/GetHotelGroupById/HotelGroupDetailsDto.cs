namespace HotelBookingSystem.Application.Features.HotelGroups.Queries.GetHotelGroupById;

public class HotelGroupDetailsDto
{
    public Guid Id { get; set; }
    public string GroupName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
