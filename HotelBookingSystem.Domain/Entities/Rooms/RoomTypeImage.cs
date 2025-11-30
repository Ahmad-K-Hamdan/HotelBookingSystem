namespace HotelBookingSystem.Domain.Entities.Rooms;

public class RoomTypeImage
{
    public Guid Id { get; set; }
    public Guid HotelRoomTypeId { get; set; }
    public string Url { get; set; } = null!;
    public bool IsMain { get; set; }

    public HotelRoomType HotelRoomType { get; set; } = null!;
}
