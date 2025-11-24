using FluentValidation;

namespace HotelBookingSystem.Application.Features.Cities.Commands.DeleteCity;

public class DeleteCityCommandValidator : AbstractValidator<DeleteCityCommand>
{
    public DeleteCityCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("City ID is required.");
    }
}
