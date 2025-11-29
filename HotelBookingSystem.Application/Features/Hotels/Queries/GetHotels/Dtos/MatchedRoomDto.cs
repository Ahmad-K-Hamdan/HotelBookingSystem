namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels.Dtos;

public class MatchedRoomDto
{
    public Guid RoomId { get; set; }
    public string RoomTypeName { get; set; } = null!;
    public decimal OriginalPricePerNight { get; set; }
    public decimal DiscountedPricePerNight { get; set; }
    public int MaxAdults { get; set; }
    public int MaxChildren { get; set; }
}
