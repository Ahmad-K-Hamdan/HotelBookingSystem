namespace HotelBookingSystem.Application.Features.HotelGroups.Queries.GetHotelGroups;

public class HotelGroupDto
{
    public Guid Id { get; set; }
    public string GroupName { get; set; } = null!;
    public string? Description { get; set; }
}
