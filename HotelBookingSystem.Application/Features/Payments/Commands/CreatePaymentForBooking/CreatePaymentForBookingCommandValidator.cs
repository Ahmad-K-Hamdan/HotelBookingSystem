using FluentValidation;

namespace HotelBookingSystem.Application.Features.Payments.Commands.CreatePaymentForBooking;

public class CreatePaymentForBookingCommandValidator : AbstractValidator<CreatePaymentForBookingCommand>
{
    public CreatePaymentForBookingCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("Booking ID is required.");

        RuleFor(x => x.PaymentMethodId)
            .NotEmpty().WithMessage("Payment method ID is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0m)
            .WithMessage("Payment amount must be greater than zero.");
    }
}
