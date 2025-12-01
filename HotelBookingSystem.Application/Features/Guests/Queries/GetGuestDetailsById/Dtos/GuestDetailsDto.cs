namespace HotelBookingSystem.Application.Features.Guests.Queries.GetGuestDetailsById.Dtos;

public class GuestDetailsDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = null!;
    public string PassportNumber { get; set; } = null!;
    public string HomeCountry { get; set; } = null!;

    public List<GuestReviewDto> Reviews { get; set; } = new();
    public List<GuestBookingDto> Bookings { get; set; } = new();
}
