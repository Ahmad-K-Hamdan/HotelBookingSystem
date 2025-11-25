using FluentValidation;

namespace HotelBookingSystem.Application.Features.PaymentMethods.Commands.DeletePaymentMethod;

public class DeletePaymentMethodCommandValidator : AbstractValidator<DeletePaymentMethodCommand>
{
    public DeletePaymentMethodCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Payment method ID is required.");
    }
}
