namespace HotelBookingSystem.Application.Features.HotelRoomTypes.Queries.GetHotelRoomTypes;

public class HotelRoomTypeListDto
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public string HotelName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal PricePerNight { get; set; }
    public int BedsCount { get; set; }
    public int MaxNumOfGuestsAdults { get; set; }
    public int MaxNumOfGuestsChildren { get; set; }
    public int RoomsCount { get; set; }
}
