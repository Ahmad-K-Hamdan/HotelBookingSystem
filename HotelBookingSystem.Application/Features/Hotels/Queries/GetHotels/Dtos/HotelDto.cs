namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels.Dtos;

public class HotelDto
{
    public Guid Id { get; set; }
    public string HotelName { get; set; } = null!;
    public string HotelGroup { get; set; } = null!;
    public string HotelAddress { get; set; } = null!;
    public string CityName { get; set; } = null!;
    public string CountryName { get; set; } = null!;
    public int StarRating { get; set; }
    public bool HasActiveDiscount { get; set; }
    public decimal? MinOriginalPricePerNight { get; set; }
    public decimal? MinDiscountedPricePerNight { get; set; }
    public int VisitCount { get; set; }
    public string? MainImageUrl { get; set; }
    public List<string> AmenityNames { get; set; } = new();
    public List<MatchedRoomDto> MatchedRooms { get; set; } = new();
}
