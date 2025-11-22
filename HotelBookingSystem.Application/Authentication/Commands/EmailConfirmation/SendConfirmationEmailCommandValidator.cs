using FluentValidation;

namespace HotelBookingSystem.Application.Authentication.Commands.EmailConfirmation;

public class SendConfirmationEmailCommandValidator : AbstractValidator<SendConfirmationEmailCommand>
{
    public SendConfirmationEmailCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
