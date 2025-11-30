using HotelBookingSystem.Domain.Entities.Rooms;

namespace HotelBookingSystem.Application.Features.HotelRooms.Queries.GetHotelRoomById.Dtos;

public class RoomTypeForRoomDto
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public string RoomTypeName { get; set; } = null!;
    public string? Description { get; set; }
    public decimal PricePerNight { get; set; }
    public int BedsCount { get; set; }
    public int MaxNumOfGuestsAdults { get; set; }
    public int MaxNumOfGuestsChildren { get; set; }
    public List<RoomTypeImageDto> Images { get; set; } = new();
}
