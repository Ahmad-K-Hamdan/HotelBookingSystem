namespace HotelBookingSystem.Application.Features.RoomTypeImages.Queries.GetRoomTypeImageById;

public class RoomTypeImageDetailsDto
{
    public Guid Id { get; set; }
    public Guid HotelRoomTypeId { get; set; }
    public string RoomTypeName { get; set; } = null!;
    public Guid HotelId { get; set; }
    public string HotelName { get; set; } = null!;
    public decimal PricePerNight { get; set; }
    public int BedsCount { get; set; }
    public int MaxNumOfGuestsAdults { get; set; }
    public int MaxNumOfGuestsChildren { get; set; }

    public string Url { get; set; } = null!;
    public bool IsMain { get; set; }
}
