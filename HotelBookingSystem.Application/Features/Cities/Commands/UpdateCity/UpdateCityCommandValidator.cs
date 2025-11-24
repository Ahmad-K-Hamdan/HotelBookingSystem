using FluentValidation;

namespace HotelBookingSystem.Application.Features.Cities.Commands.UpdateCity;

public class UpdateCityCommandValidator : AbstractValidator<UpdateCityCommand>
{
    public UpdateCityCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("City ID is required.");

        RuleFor(x => x.CityName)
            .NotEmpty().WithMessage("City name is required.")
            .Matches("^[A-Za-z ]+$").WithMessage("City name must contain only letters and spaces.")
            .MaximumLength(200).WithMessage("City name must not exceed 200 characters.")
            .Must(x => x.Trim() == x).WithMessage("City name cannot start or end with spaces.");

        RuleFor(x => x.CountryName)
            .NotEmpty().WithMessage("Country name is required.")
            .Matches("^[A-Za-z ]+$").WithMessage("Country name must contain only letters and spaces.")
            .MaximumLength(200).WithMessage("Country name must not exceed 200 characters.")
            .Must(x => x.Trim() == x).WithMessage("Country name cannot start or end with spaces.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.")
            .Matches("^[A-Za-z0-9 !?,._'\"()\\-/]*$")
            .WithMessage("password contains illegal characters. Only English letters, numbers, and punctuation are allowed.")
            .Must(x => x == null || x.Trim() == x)
            .WithMessage("Description cannot start or end with spaces.");
    }
}
