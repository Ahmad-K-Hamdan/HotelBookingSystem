namespace HotelBookingSystem.Application.Features.HotelRooms.Queries.GetHotelRooms;

public class HotelRoomListDto
{
    public Guid Id { get; set; }
    public Guid HotelRoomTypeId { get; set; }
    public int RoomNumber { get; set; }
    public bool IsAvailable { get; set; }

    public Guid HotelId { get; set; }
    public string HotelName { get; set; } = null!;
    public string RoomTypeName { get; set; } = null!;
}
