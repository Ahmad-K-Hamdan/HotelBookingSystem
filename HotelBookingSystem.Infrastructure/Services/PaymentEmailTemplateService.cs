using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Bookings;
using HotelBookingSystem.Domain.Entities.Payments;
using System.Text;

namespace HotelBookingSystem.Infrastructure.Services
{
    public class PaymentEmailTemplateService : IPaymentEmailTemplateService
    {
        public string GeneratePaymentConfirmationEmail(
            Booking booking,
            Payment payment,
            decimal totalDue,
            decimal totalPaid,
            decimal remaining)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<html><body>");
            sb.AppendLine("<h2>Payment Confirmation</h2>");
            sb.AppendLine("<p>Dear customer,</p>");
            sb.AppendLine("<p>Your payment has been received successfully.</p>");

            sb.AppendLine("<h3>Booking Details</h3>");
            sb.AppendLine("<ul>");
            sb.AppendLine($"<li><strong>Confirmation Code:</strong> {booking.ConfirmationCode}</li>");
            sb.AppendLine($"<li><strong>Hotel:</strong> {booking.Hotel.HotelName}</li>");
            sb.AppendLine($"<li><strong>Address:</strong> {booking.Hotel.HotelAddress}, {booking.Hotel.City.CityName}, {booking.Hotel.City.CountryName}</li>");
            sb.AppendLine($"<li><strong>Check-in:</strong> {booking.CheckInDate:yyyy-MM-dd}</li>");
            sb.AppendLine($"<li><strong>Check-out:</strong> {booking.CheckOutDate:yyyy-MM-dd}</li>");
            sb.AppendLine($"<li><strong>Nights:</strong> {booking.Nights}</li>");
            sb.AppendLine("</ul>");

            sb.AppendLine("<h3>Payment Details</h3>");
            sb.AppendLine("<ul>");
            sb.AppendLine($"<li><strong>Payment Amount:</strong> {payment.PaymentAmount:C}</li>");
            sb.AppendLine($"<li><strong>Total Due:</strong> {totalDue:C}</li>");
            sb.AppendLine($"<li><strong>Total Paid:</strong> {totalPaid:C}</li>");
            sb.AppendLine($"<li><strong>Outstanding Balance:</strong> {remaining:C}</li>");
            sb.AppendLine($"<li><strong>Status:</strong> {payment.PaymentStatus}</li>");
            sb.AppendLine($"<li><strong>Payment Date (UTC):</strong> {payment.PaymentDate:yyyy-MM-dd HH:mm}</li>");
            sb.AppendLine("</ul>");

            sb.AppendLine("<p>Thank you for booking with us.</p>");
            sb.AppendLine("</body></html>");

            return sb.ToString();
        }
    }
}
