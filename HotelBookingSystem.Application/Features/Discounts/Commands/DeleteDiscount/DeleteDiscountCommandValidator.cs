using FluentValidation;

namespace HotelBookingSystem.Application.Features.Discounts.Commands.DeleteDiscount;

public class DeleteDiscountCommandValidator : AbstractValidator<DeleteDiscountCommand>
{
    public DeleteDiscountCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Discount ID is required.");
    }
}
