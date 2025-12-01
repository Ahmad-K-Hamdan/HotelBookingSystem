namespace HotelBookingSystem.Application.Features.Bookings.Queries.GetBookingById.Dtos;

public class BookingDetailsDto
{
    public Guid Id { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public int Nights { get; set; }
    public int TotalAdults { get; set; }
    public int TotalChildren { get; set; }
    public string ConfirmationCode { get; set; } = null!;
    public string? SpecialRequests { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalOriginalPrice { get; set; }
    public decimal TotalDiscountedPrice { get; set; }
    public Guid HotelId { get; set; }
    public string HotelName { get; set; } = null!;
    public string CityName { get; set; } = null!;
    public string CountryName { get; set; } = null!;
    public int StarRating { get; set; }
    public Guid GuestId { get; set; }
    public string GuestHomeCountry { get; set; } = null!;

    public List<BookingRoomDto> Rooms { get; set; } = new();
}
