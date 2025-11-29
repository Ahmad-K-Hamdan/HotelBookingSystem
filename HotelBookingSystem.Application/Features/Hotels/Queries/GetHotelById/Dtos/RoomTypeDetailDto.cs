namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotelById.Dtos;

public class RoomTypeDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int BedsCount { get; set; }
    public int MaxAdults { get; set; }
    public int MaxChildren { get; set; }
    public bool IsAvailable { get; set; }
    public decimal OriginalPricePerNight { get; set; }
    public decimal DiscountedPricePerNight { get; set; }
    public List<string> ImageUrls { get; set; } = new();
}
