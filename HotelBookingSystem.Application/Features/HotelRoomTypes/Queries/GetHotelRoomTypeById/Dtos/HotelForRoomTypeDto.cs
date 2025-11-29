namespace HotelBookingSystem.Application.Features.HotelRoomTypes.Queries.GetHotelRoomTypeById.Dtos;

public class HotelForRoomTypeDto
{
    public Guid Id { get; set; }
    public string HotelName { get; set; } = null!;
    public string CityName { get; set; } = null!;
    public string CountryName { get; set; } = null!;
    public int StarRating { get; set; }
}
