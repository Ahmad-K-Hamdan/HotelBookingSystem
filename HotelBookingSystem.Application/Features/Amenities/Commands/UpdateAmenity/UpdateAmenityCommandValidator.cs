using FluentValidation;

namespace HotelBookingSystem.Application.Features.Amenities.Commands.UpdateAmenity;

public class UpdateAmenityCommandValidator : AbstractValidator<UpdateAmenityCommand>
{
    public UpdateAmenityCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Amenity ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Amenity name is required.")
            .Matches("^[A-Za-z ]+$").WithMessage("Amenity name must contain only letters and spaces.")
            .MaximumLength(200).WithMessage("Amenity name must not exceed 200 characters.")
            .Must(x => x.Trim() == x).WithMessage("Amenity name cannot start or end with spaces.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.")
            .Matches("^[A-Za-z0-9 !?,._'\"()\\-/]*$")
            .WithMessage("password contains illegal characters. Only English letters, numbers, and punctuation are allowed.")
            .Must(x => x == null || x.Trim() == x)
            .WithMessage("Description cannot start or end with spaces.");
    }
}
