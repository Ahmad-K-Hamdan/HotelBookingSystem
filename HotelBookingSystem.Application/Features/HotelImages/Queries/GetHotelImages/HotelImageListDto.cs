namespace HotelBookingSystem.Application.Features.HotelImages.Queries.GetHotelImages;

public class HotelImageListDto
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public string HotelName { get; set; } = null!;
    public string Url { get; set; } = null!;
    public bool IsMain { get; set; }
}
