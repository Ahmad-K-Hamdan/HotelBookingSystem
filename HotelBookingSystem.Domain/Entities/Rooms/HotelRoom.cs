using HotelBookingSystem.Domain.Entities.Bookings;

namespace HotelBookingSystem.Domain.Entities.Rooms;

public class HotelRoom
{
    public Guid Id { get; set; }
    public Guid HotelRoomTypeId { get; set; }
    public int RoomNumber { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public HotelRoomType RoomType { get; set; } = null!;
    public ICollection<BookingRoom> BookingRooms { get; set; } = new List<BookingRoom>();
}
