using MediatR;

namespace HotelBookingSystem.Application.Features.Bookings.Queries.GetMyBookings;

public class GetMyBookingsQuery : IRequest<List<BookingDto>>
{
    public bool UpcomingOnly { get; set; }
    public bool PastOnly { get; set; }
}
