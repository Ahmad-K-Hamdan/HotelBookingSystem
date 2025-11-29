namespace HotelBookingSystem.Application.Features.Guests.Queries.GetGuests;

public class GuestListDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = null!;
    public string PassportNumber { get; set; } = null!;
    public string HomeCountry { get; set; } = null!;
}
