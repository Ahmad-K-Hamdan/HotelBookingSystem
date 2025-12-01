namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetRecentlyVisited;

public class RecentHotelDto
{
    public Guid HotelId { get; set; }
    public string HotelName { get; set; } = null!;
    public string CityName { get; set; } = null!;
    public string CountryName { get; set; } = null!;
    public int StarRating { get; set; }
    public string? MainImageUrl { get; set; }
    public decimal? MinOriginalPricePerNight { get; set; }
    public decimal? MinDiscountedPricePerNight { get; set; }
    public DateTime LastVisitedAt { get; set; }
}
