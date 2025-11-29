namespace HotelBookingSystem.Application.Features.Reviews.Queries.GetReviews;

public class ReviewListDto
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public string HotelName { get; set; } = null!;
    public Guid GuestId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime ReviewDate { get; set; }
}
