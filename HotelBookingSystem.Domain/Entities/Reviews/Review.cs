using HotelBookingSystem.Domain.Entities.Guests;
using HotelBookingSystem.Domain.Entities.Hotels;

namespace HotelBookingSystem.Domain.Entities.Reviews;

public class Review
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public Guid GuestId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime ReviewDate { get; set; }

    public Hotel Hotel { get; set; } = null!;
    public Guest Guest { get; set; } = null!;
}
