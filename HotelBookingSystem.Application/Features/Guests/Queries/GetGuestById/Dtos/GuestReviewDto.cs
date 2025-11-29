namespace HotelBookingSystem.Application.Features.Guests.Queries.GetGuestById.Dtos;

public class GuestReviewDto
{
    public Guid ReviewId { get; set; }
    public Guid HotelId { get; set; }
    public string HotelName { get; set; } = null!;
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime ReviewDate { get; set; }
}
