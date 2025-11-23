using HotelBookingSystem.Domain.Entities.Hotels;

namespace HotelBookingSystem.Domain.Entities.Rooms;

public class HotelRoomType
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal PricePerNight { get; set; }
    public int BedsCount { get; set; }
    public int MaxNumOfGuestsAdults { get; set; }
    public int MaxNumOfGuestsChildren { get; set; }

    public Hotel Hotel { get; set; } = null!;
    public ICollection<HotelRoom> Rooms { get; set; } = new List<HotelRoom>();
}
