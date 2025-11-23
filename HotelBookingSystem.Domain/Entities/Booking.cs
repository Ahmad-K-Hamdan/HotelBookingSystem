namespace HotelBookingSystem.Domain.Entities;

public class Booking
{
    public Guid Id { get; set; }
    public Guid GuestId { get; set; }
    public Guid HotelId { get; set; }
    public Guid HotelRoomId { get; set; }
    public DateTime CheckInDateTime { get; set; }
    public DateTime CheckOutDateTime { get; set; }
    public int NumOfAdults { get; set; }
    public int NumOfChildren { get; set; }
    public string? SpecialRequests { get; set; }
    public string ConfirmationCode { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public Guest Guest { get; set; } = null!;
    public Hotel Hotel { get; set; } = null!;
    public HotelRoom HotelRoom { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
