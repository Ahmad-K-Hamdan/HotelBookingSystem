namespace HotelBookingSystem.Application.Features.Reviews.Queries.GetReviewById.Dtos;

public class HotelSummaryDto
{
    public Guid Id { get; set; }
    public string HotelName { get; set; } = null!;
    public string CityName { get; set; } = null!;
    public string CountryName { get; set; } = null!;
    public int StarRating { get; set; }
}
