using FluentValidation;
using HotelBookingSystem.Application.Features.HotelGroups.Commands.UpdateHotelGroup;

namespace HotelBookingSystem.Application.Features.Discounts.Commands.UpdateDiscount;

public class UpdateHotelGroupCommandValidator : AbstractValidator<UpdateHotelGroupCommand>
{
    public UpdateHotelGroupCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Amenity ID is required.");

        RuleFor(x => x.GroupName)
            .NotEmpty().WithMessage("Group name is required.")
            .Matches("^[A-Za-z0-9 !?,._'\"()\\-/]*$")
            .WithMessage("Group name contains illegal characters. Only English letters, numbers, and punctuation are allowed.")
            .MaximumLength(200).WithMessage("Group name must not exceed 200 characters.")
            .Must(x => x.Trim() == x).WithMessage("Group name cannot start or end with spaces.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.")
            .Matches("^[A-Za-z0-9 !?,._'\"()\\-/]*$")
            .WithMessage("Description contains illegal characters. Only English letters, numbers, and punctuation are allowed.")
            .Must(x => x == null || x.Trim() == x)
            .WithMessage("Description cannot start or end with spaces.");
    }
}
