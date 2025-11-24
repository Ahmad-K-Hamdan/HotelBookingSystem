using FluentValidation;

namespace HotelBookingSystem.Application.Features.Amenities.Commands.DeleteAmenity;

public class DeleteAmenityCommandValidator : AbstractValidator<DeleteAmenityCommand>
{
    public DeleteAmenityCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Amenity ID is required.");
    }
}
