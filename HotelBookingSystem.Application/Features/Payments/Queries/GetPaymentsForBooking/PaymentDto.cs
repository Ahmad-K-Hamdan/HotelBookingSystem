using HotelBookingSystem.Domain.Enums;

namespace HotelBookingSystem.Application.Features.Payments.Queries.GetPaymentsForBooking;

public class PaymentDto
{
    public Guid Id { get; set; }
    public decimal PaymentAmount { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public DateTime PaymentDate { get; set; }
    public Guid PaymentMethodId { get; set; }
}
