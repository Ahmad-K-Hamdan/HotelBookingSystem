using HotelBookingSystem.Domain.Enums;

namespace HotelBookingSystem.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public Guid? DiscountId { get; set; }
    public Guid PaymentMethodId { get; set; }
    public decimal PaymentAmount { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public DateTime PaymentDate { get; set; }

    public Booking Booking { get; set; } = null!;
    public Discount? Discount { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = null!;
}
