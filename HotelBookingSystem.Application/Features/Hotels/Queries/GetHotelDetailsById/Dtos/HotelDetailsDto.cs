namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotelDetailsById.Dtos;

public class HotelDetailsDto
{
    public Guid Id { get; set; }
    public string HotelName { get; set; } = null!;
    public string HotelGroup { get; set; } = null!;
    public string HotelAddress { get; set; } = null!;
    public string CityName { get; set; } = null!;
    public string CountryName { get; set; } = null!;
    public int StarRating { get; set; }
    public string? Description { get; set; }
    public bool HasActiveDiscount { get; set; }
    public decimal? DiscountRate { get; set; }
    public List<string> ImageUrls { get; set; } = new();
    public List<string> Amenities { get; set; } = new();
    public List<RoomTypeDetailDto> RoomTypes { get; set; } = new();
    public List<ReviewDto> Reviews { get; set; } = new();
    public double? AverageRating { get; set; }
    public int ReviewCount { get; set; }
}
