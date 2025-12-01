using HotelBookingSystem.Domain.Entities.Guests;
using HotelBookingSystem.Domain.Entities.Hotels;
using HotelBookingSystem.Domain.Entities.Payments;

namespace HotelBookingSystem.Domain.Entities.Bookings;

public class Booking
{
    public Guid Id { get; set; }
    public Guid GuestId { get; set; }
    public Guid HotelId { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public int Nights { get; set; }
    public int TotalAdults { get; set; }
    public int TotalChildren { get; set; }
    public string? SpecialRequests { get; set; }
    public string ConfirmationCode { get; set; } = null!;
    public decimal TotalOriginalPrice { get; set; }
    public decimal TotalDiscountedPrice { get; set; }
    public DateTime CreatedAt { get; set; }

    public Guest Guest { get; set; } = null!;
    public Hotel Hotel { get; set; } = null!;
    public ICollection<BookingRoom> BookingRooms { get; set; } = new List<BookingRoom>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
