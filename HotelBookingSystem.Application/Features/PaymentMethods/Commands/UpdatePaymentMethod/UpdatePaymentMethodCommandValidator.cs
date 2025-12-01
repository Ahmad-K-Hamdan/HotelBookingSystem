using FluentValidation;

namespace HotelBookingSystem.Application.Features.PaymentMethods.Commands.UpdatePaymentMethod;

public class UpdatePaymentMethodCommandValidator : AbstractValidator<UpdatePaymentMethodCommand>
{
    public UpdatePaymentMethodCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Amenity ID is required.");

        RuleFor(x => x.MethodName)
            .NotEmpty().WithMessage("Method name is required.")
            .Matches("^[A-Za-z ]+$").WithMessage("Method name must contain only letters and spaces.")
            .MaximumLength(200).WithMessage("Method name must not exceed 200 characters.")
            .Must(x => x.Trim() == x).WithMessage("Method name cannot start or end with spaces.");
    }
}
