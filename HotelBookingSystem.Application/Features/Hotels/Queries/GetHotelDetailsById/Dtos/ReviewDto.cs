namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotelDetailsById.Dtos;

public class ReviewDto
{
    public Guid Id { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = null!;
    public DateTime ReviewDate { get; set; }
    public string GuestName { get; set; } = null!;
}
