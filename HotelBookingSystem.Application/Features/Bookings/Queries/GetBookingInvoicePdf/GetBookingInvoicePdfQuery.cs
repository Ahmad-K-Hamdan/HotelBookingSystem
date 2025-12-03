using MediatR;

namespace HotelBookingSystem.Application.Features.Bookings.Queries.GetBookingInvoicePdf
{
    public record GetBookingInvoicePdfQuery(Guid BookingId) : IRequest<byte[]>;
}
