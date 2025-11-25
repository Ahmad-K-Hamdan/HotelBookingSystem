using FluentValidation;

namespace HotelBookingSystem.Application.Features.HotelGroups.Commands.DeleteHotelGroup;

public class DeleteHotelGroupCommandValidator : AbstractValidator<DeleteHotelGroupCommand>
{
    public DeleteHotelGroupCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Hotel Group ID is required.");
    }
}
