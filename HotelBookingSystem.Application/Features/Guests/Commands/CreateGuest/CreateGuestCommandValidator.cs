using FluentValidation;

namespace HotelBookingSystem.Application.Features.Guests.Commands.CreateGuest;

public class CreateGuestCommandValidator : AbstractValidator<CreateGuestCommand>
{
    public CreateGuestCommandValidator()
    {
        RuleFor(x => x.PassportNumber)
            .NotEmpty().WithMessage("Passport number is required.")
            .MaximumLength(50).WithMessage("Passport number must not exceed 50 characters.")
            .Matches("^[A-Za-z0-9-]+$")
            .WithMessage("Passport number must contain only letters, numbers, and dashes.")
            .Must(x => x.Trim() == x).WithMessage("Passport number cannot start or end with spaces.");

        RuleFor(x => x.HomeCountry)
            .NotEmpty().WithMessage("Home country is required.")
            .MaximumLength(100).WithMessage("Home country must not exceed 100 characters.")
            .Matches("^[A-Za-z ]+$")
            .WithMessage("Home country must contain only letters and spaces.")
            .Must(x => x.Trim() == x)
            .WithMessage("Home country cannot start or end with spaces.");
    }
}
