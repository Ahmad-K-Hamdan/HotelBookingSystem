using HotelBookingSystem.Domain.Entities.Hotels;
using HotelBookingSystem.Domain.Entities.Rooms;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels;

public class HotelAssignment
{
    public Hotel Hotel { get; set; } = null!;
    public List<(HotelRoom Room, HotelRoomType Type)> AssignedRooms { get; set; } = new();
    public decimal BasePricePerNight { get; set; }
    public decimal DiscountedPricePerNight { get; set; }
    public decimal TotalDiscountedPrice { get; set; }
}
