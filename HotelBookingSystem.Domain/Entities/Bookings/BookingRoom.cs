using HotelBookingSystem.Domain.Entities.Rooms;

namespace HotelBookingSystem.Domain.Entities.Bookings;

public class BookingRoom
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public Guid HotelRoomId { get; set; }
    public int NumOfAdults { get; set; }
    public int NumOfChildren { get; set; }
    public decimal PricePerNightOriginal { get; set; }
    public decimal PricePerNightDiscounted { get; set; }

    public Booking Booking { get; set; } = null!;
    public HotelRoom HotelRoom { get; set; } = null!;
}
