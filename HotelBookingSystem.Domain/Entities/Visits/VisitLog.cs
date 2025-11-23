using HotelBookingSystem.Domain.Entities.Hotels;

namespace HotelBookingSystem.Domain.Entities.Visits;

public class VisitLog
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = null!;
    public Guid HotelId { get; set; }
    public DateTime VisitedAt { get; set; }

    public Hotel Hotel { get; set; } = null!;
}
