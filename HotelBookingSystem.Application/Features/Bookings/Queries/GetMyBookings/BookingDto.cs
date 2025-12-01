namespace HotelBookingSystem.Application.Features.Bookings.Queries.GetMyBookings;

public class BookingDto
{
    public Guid Id { get; set; }
    public string HotelName { get; set; } = null!;
    public string CityName { get; set; } = null!;
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public int Nights { get; set; }
    public string ConfirmationCode { get; set; } = null!;
    public decimal TotalOriginalPrice { get; set; }
    public decimal TotalDiscountedPrice { get; set; }
}
