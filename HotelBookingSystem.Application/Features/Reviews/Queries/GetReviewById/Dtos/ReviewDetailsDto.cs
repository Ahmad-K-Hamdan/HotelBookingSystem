namespace HotelBookingSystem.Application.Features.Reviews.Queries.GetReviewById.Dtos;

public class ReviewDetailsDto
{
    public Guid Id { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime ReviewDate { get; set; }

    public HotelSummaryDto Hotel { get; set; } = null!;
    public GuestSummaryDto Guest { get; set; } = null!;
}
