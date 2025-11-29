namespace HotelBookingSystem.Application.Features.Reviews.Queries.GetReviewById.Dtos;

public class GuestSummaryDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = null!;
    public string HomeCountry { get; set; } = null!;
}
