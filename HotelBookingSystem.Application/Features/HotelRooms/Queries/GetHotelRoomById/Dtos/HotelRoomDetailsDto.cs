namespace HotelBookingSystem.Application.Features.HotelRooms.Queries.GetHotelRoomById.Dtos;

public class HotelRoomDetailsDto
{
    public Guid Id { get; set; }
    public Guid HotelRoomTypeId { get; set; }
    public int RoomNumber { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public RoomTypeForRoomDto RoomType { get; set; } = null!;
}
