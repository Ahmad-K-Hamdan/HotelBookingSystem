using MediatR;

namespace HotelBookingSystem.Application.Features.Payments.Commands.CreatePaymentForBooking;

public class CreatePaymentForBookingCommand : IRequest<Guid>
{
    public Guid BookingId { get; set; }
    public Guid PaymentMethodId { get; set; }
    public decimal Amount { get; set; }
}
