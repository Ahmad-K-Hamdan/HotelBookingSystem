using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotelById.Dtos;
using MediatR;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotelById;

public class GetHotelByIdQuery : IRequest<HotelDetailsDto>
{
    public GetHotelByIdQuery(Guid id, DateOnly? checkIn, DateOnly? checkOut)
    {
        Id = id;
        CheckInDate = checkIn ?? DateOnly.FromDateTime(DateTime.Today);
        CheckOutDate = checkOut ?? DateOnly.FromDateTime(DateTime.Today.AddDays(1));
    }

    public Guid Id { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
}
