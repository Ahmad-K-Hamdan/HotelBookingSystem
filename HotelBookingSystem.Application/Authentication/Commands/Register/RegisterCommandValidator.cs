using FluentValidation;

namespace HotelBookingSystem.Application.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100)
            .Matches("^[A-Za-z]+$").WithMessage("First name must contain only letters.")
            .Must(x => x.Trim() == x).WithMessage("First name cannot start or end with spaces."); ;

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100)
            .Matches("^[A-Za-z]+$").WithMessage("Last name must contain only letters.")
            .Must(x => x.Trim() == x).WithMessage("Last name cannot start or end with spaces."); ;

        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty()
            .Must(x => x == x.Trim()).WithMessage("Email cannot contain leading or trailing spaces.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.")
            .Matches("[@!_.,-]").WithMessage("Password must contain at least one special character.")
            .Matches("^\\S+$").WithMessage("Password cannot contain spaces.")
            .Matches("^[A-Za-z0-9@!_.,-]*$")
            .WithMessage("Password contains illegal characters. Only English letters, numbers, and (@, !, _, ., -) are allowed.");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Birth Date is required.")
            .Must(date => date <= DateTime.Today.AddYears(-18)).WithMessage("You must be at least 18 years old.");
    }
}
