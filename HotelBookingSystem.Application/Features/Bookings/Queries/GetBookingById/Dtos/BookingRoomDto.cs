namespace HotelBookingSystem.Application.Features.Bookings.Queries.GetBookingById.Dtos;

public class BookingRoomDto
{
    public Guid BookingRoomId { get; set; }
    public Guid HotelRoomId { get; set; }
    public string RoomTypeName { get; set; } = null!;
    public int RoomNumber { get; set; }
    public int Adults { get; set; }
    public int Children { get; set; }
    public decimal PricePerNightOriginal { get; set; }
    public decimal PricePerNightDiscounted { get; set; }
}
