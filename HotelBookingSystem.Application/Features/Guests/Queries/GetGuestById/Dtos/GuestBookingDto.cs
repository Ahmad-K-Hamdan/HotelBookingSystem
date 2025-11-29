namespace HotelBookingSystem.Application.Features.Guests.Queries.GetGuestById.Dtos;

public class GuestBookingDto
{
    public Guid BookingId { get; set; }
    public Guid HotelId { get; set; }
    public string HotelName { get; set; } = null!;
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public int NumOfAdults { get; set; }
    public int NumOfChildren { get; set; }
    public string? SpecialRequests { get; set; }
    public string ConfirmationCode { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
