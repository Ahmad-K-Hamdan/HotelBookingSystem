using FluentValidation;

namespace HotelBookingSystem.Application.Authentication.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty()
            .Must(x => x == x.Trim()).WithMessage("Email cannot contain leading or trailing spaces.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches("^[A-Za-z0-9@!_.,-]*$")
            .WithMessage("Password contains illegal characters. Only English letters, numbers, and (@, !, _, ., -) are allowed.");
    }
}
