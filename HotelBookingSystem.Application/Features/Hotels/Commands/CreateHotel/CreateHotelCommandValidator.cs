using FluentValidation;

namespace HotelBookingSystem.Application.Features.Hotels.Commands.CreateHotel;

public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelCommandValidator()
    {
        RuleFor(x => x.Hotel.HotelGroupId)
         .NotEmpty().WithMessage("Hotel group ID is required.");

        RuleFor(x => x.Hotel.CityId)
         .NotEmpty().WithMessage("City ID is required.");

        RuleFor(x => x.Hotel.HotelName)
            .NotEmpty().WithMessage("Hotel name is required.")
            .Matches("^[A-Za-z0-9 !?,._'\"()\\-/]*$")
            .WithMessage("Hotel name contains illegal characters. Only English letters, numbers, and punctuation are allowed.")
            .MaximumLength(200).WithMessage("Hotel name must not exceed 200 characters.")
            .Must(x => x.Trim() == x).WithMessage("Hotel name cannot start or end with spaces.");

        RuleFor(x => x.Hotel.HotelAddress)
            .NotEmpty().WithMessage("Hotel address is required.")
            .Matches("^[A-Za-z0-9 !?,._'\"()\\-/]*$")
            .WithMessage("Hotel address contains illegal characters. Only English letters, numbers, and punctuation are allowed.")
            .MaximumLength(500).WithMessage("Hotel address must not exceed 500 characters.")
            .Must(x => x.Trim() == x).WithMessage("Hotel address cannot start or end with spaces.");

        RuleFor(x => x.Hotel.StarRating)
            .NotNull().WithMessage("Hotel star rating is required.")
            .InclusiveBetween(1, 5).WithMessage("Hotel star rating must be between 1 and 5.");

        RuleFor(x => x.Hotel.Latitude)
            .NotNull().WithMessage("Hotel latitude is required.")
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

        RuleFor(x => x.Hotel.Longitude)
            .NotNull().WithMessage("Hotel longitude is required.")
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");

        RuleFor(x => x.Hotel.Description)
            .NotEmpty().WithMessage("Description is required.")
            .Matches("^[A-Za-z0-9 !?,._'\"()\\-/]*$")
            .WithMessage("Description contains illegal characters. Only English letters, numbers, and punctuation are allowed.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .Must(x => x == null || x.Trim() == x)
            .WithMessage("Description cannot start or end with spaces.");
    }
}
