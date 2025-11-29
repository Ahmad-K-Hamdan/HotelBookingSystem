namespace HotelBookingSystem.Application.Features.HotelRoomTypes.Queries.GetHotelRoomTypeById.Dtos;

public class RoomInRoomTypeDto
{
    public Guid Id { get; set; }
    public int RoomNumber { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedAt { get; set; }
}
