namespace HotelBookingSystem.Domain.Entities;

public class HotelImage
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public string Url { get; set; } = null!;
    public bool IsMain { get; set; }

    public Hotel Hotel { get; set; } = null!;
}
