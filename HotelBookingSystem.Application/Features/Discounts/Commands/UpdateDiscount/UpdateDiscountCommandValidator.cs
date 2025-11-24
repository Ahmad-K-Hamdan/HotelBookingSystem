using FluentValidation;

namespace HotelBookingSystem.Application.Features.Discounts.Commands.UpdateDiscount;

public class UpdateDiscountCommandValidator : AbstractValidator<UpdateDiscountCommand>
{
    public UpdateDiscountCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Amenity ID is required.");

        RuleFor(x => x.DiscountDescription)
            .NotEmpty().WithMessage("Discount description is required.")
            .Matches("^[A-Za-z0-9 !?,._'\"()\\-/]*$")
            .WithMessage("Discount description contains illegal characters. Only English letters, numbers, and punctuation are allowed.")
            .MaximumLength(500).WithMessage("Discount description must not exceed 500 characters.")
            .Must(x => x.Trim() == x).WithMessage("Discount description cannot start or end with spaces.");

        RuleFor(x => x.DiscountRate)
            .NotEmpty().WithMessage("Discount rate is required.")
            .InclusiveBetween(0.01m, 1.00m).WithMessage("Discount rate must be between 1% (0.01) and 100% (1.00).");

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("Active status is required.");
    }
}
