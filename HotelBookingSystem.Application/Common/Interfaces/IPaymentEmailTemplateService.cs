using HotelBookingSystem.Domain.Entities.Bookings;
using HotelBookingSystem.Domain.Entities.Payments;

namespace HotelBookingSystem.Application.Common.Interfaces
{
    public interface IPaymentEmailTemplateService
    {
        string GeneratePaymentConfirmationEmail(
            Booking booking,
            Payment payment,
            decimal totalDue,
            decimal totalPaid,
            decimal remaining);
    }
}
