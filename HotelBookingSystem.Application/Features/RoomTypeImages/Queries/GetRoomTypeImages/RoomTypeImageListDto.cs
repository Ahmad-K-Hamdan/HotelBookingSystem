namespace HotelBookingSystem.Application.Features.RoomTypeImages.Queries.GetRoomTypeImages;

public class RoomTypeImageListDto
{
    public Guid Id { get; set; }
    public Guid HotelRoomTypeId { get; set; }
    public string RoomTypeName { get; set; } = null!;
    public Guid HotelId { get; set; }
    public string HotelName { get; set; } = null!;
    public string Url { get; set; } = null!;
    public bool IsMain { get; set; }
}
