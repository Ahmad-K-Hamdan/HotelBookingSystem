using FluentValidation;

namespace HotelBookingSystem.Application.Authentication.Commands.PasswordReset;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress();

        RuleFor(x => x.Token)
            .NotEmpty();

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(8)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.")
            .Matches("[@!_.,-]").WithMessage("Password must contain at least one special character.")
            .Matches("^\\S+$").WithMessage("Password cannot contain spaces.")
            .Matches("^[A-Za-z0-9@!_.,-]*$")
            .WithMessage("Password contains illegal characters. Only English letters, numbers, and (@, !, _, ., -) are allowed.");
    }
}
