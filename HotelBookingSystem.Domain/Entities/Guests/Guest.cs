using HotelBookingSystem.Domain.Entities.Bookings;
using HotelBookingSystem.Domain.Entities.Reviews;

namespace HotelBookingSystem.Domain.Entities.Guests;

public class Guest
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = null!;
    public string? PassportNumber { get; set; }
    public string? HomeCountry { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
