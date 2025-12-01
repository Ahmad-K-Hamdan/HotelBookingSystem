namespace HotelBookingSystem.Application.Features.Bookings.Commands.CreateBooking;

public class BookingRoomRequestDto
{
    public Guid HotelRoomTypeId { get; set; }
    public int Adults { get; set; }
    public int Children { get; set; }
}
