using HotelBookingSystem.Application.Features.Bookings.Queries.GetBookingDetailsById.Dtos;

namespace HotelBookingSystem.Application.Common.Interfaces
{
    public interface IBookingInvoicePdfGenerator
    {
        byte[] Generate(BookingDetailsDto booking);
    }
}
