namespace HotelBookingSystem.Application.Features.HotelRoomTypes.Queries.GetHotelRoomTypeById.Dtos;

public class HotelRoomTypeDetailsDto
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal PricePerNight { get; set; }
    public int BedsCount { get; set; }
    public int MaxNumOfGuestsAdults { get; set; }
    public int MaxNumOfGuestsChildren { get; set; }

    public HotelForRoomTypeDto Hotel { get; set; } = null!;
    public List<RoomInRoomTypeDto> Rooms { get; set; } = new();
    public List<RoomTypeImageDto> Images { get; set; } = new();
}
