using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotelDetailsById.Dtos;
using MediatR;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotelDetailsById;

public class GetHotelDetailsByIdQuery : IRequest<HotelDetailsDto>
{
    public GetHotelDetailsByIdQuery(Guid id, DateOnly? checkIn, DateOnly? checkOut)
    {
        Id = id;
        CheckInDate = checkIn ?? DateOnly.FromDateTime(DateTime.Today);
        CheckOutDate = checkOut ?? DateOnly.FromDateTime(DateTime.Today.AddDays(1));
    }

    public Guid Id { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
}
