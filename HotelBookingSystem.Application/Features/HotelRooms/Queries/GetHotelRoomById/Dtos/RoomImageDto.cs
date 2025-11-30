namespace HotelBookingSystem.Application.Features.HotelRooms.Queries.GetHotelRoomById.Dtos;

public class RoomImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = null!;
    public bool IsMain { get; set; }
}
