namespace HotelBookingSystem.Application.Features.Hotels.Commands.CreateHotel;

public class CreateHotelDto
{
    public Guid HotelGroupId { get; set; }
    public Guid CityId { get; set; }
    public Guid? DiscountId { get; set; }

    public string HotelName { get; set; } = null!;
    public string HotelAddress { get; set; } = null!;
    public int StarRating { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? Description { get; set; }
}
