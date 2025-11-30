namespace HotelBookingSystem.Application.Features.HotelImages.Queries.GetHotelImageById;

public class HotelImageDetailsDto
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public string HotelName { get; set; } = null!;
    public string HotelAddress { get; set; } = null!;
    public string Url { get; set; } = null!;
    public bool IsMain { get; set; }
}
