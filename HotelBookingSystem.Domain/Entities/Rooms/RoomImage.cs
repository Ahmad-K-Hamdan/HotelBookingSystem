namespace HotelBookingSystem.Domain.Entities.Rooms;

public class RoomImage
{
    public Guid Id { get; set; }
    public Guid HotelRoomId { get; set; }
    public string Url { get; set; } = null!;
    public bool IsMain { get; set; }

    public HotelRoom HotelRoom { get; set; } = null!;
}
