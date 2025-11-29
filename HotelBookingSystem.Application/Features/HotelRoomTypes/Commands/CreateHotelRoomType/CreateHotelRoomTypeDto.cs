namespace HotelBookingSystem.Application.Features.HotelRoomTypes.Commands.CreateHotelRoomType;

public class CreateHotelRoomTypeDto
{
    public Guid HotelId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal PricePerNight { get; set; }
    public int BedsCount { get; set; }
    public int MaxNumOfGuestsAdults { get; set; }
    public int MaxNumOfGuestsChildren { get; set; }
}
