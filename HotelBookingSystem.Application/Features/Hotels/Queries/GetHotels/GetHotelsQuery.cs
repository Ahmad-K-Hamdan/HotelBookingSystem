using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels.Dtos;
using MediatR;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels;

public class GetHotelsQuery : IRequest<List<HotelDto>>
{
    public List<RoomRequest> Rooms { get; set; } = new() { new RoomRequest { Adults = 2, Children = 1 } };
    public DateOnly CheckInDate { get; set; } = DateOnly.FromDateTime(DateTime.Today); 
    public DateOnly CheckOutDate { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddDays(1)); 
    public string? Search { get; set; }
    public string? CityName { get; set; }
    public string? CountryName { get; set; }
    public int? MinStars { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool OnlyWithActiveDiscount { get; set; }
    public Guid? HotelGroupId { get; set; }
    public List<Guid>? AmenityIds { get; set; }
    public List<string>? RoomTypes { get; set; }
    public string? Sort { get; set; }
    public int? Limit { get; set; }
}
