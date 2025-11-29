using FluentValidation;

namespace HotelBookingSystem.Application.Features.Guests.Commands.DeleteGuest;

public class DeleteGuestCommandValidator : AbstractValidator<DeleteGuestCommand>
{
    public DeleteGuestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Guest ID is required.");
    }
}
