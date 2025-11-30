using FluentValidation;

namespace HotelBookingSystem.Application.Features.HotelImages.Commands.UpdateHotelImage;

public class UpdateHotelImageCommandValidator : AbstractValidator<UpdateHotelImageCommand>
{
    public UpdateHotelImageCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Image ID is required.");

        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Hotel ID is required.");

        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Image URL is required.")
            .MaximumLength(1000).WithMessage("Image URL must not exceed 1000 characters.");
    }
}
