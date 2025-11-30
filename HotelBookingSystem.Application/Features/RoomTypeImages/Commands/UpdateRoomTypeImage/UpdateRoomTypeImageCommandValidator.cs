using FluentValidation;

namespace HotelBookingSystem.Application.Features.RoomTypeImages.Commands.UpdateRoomTypeImage;

public class UpdateRoomTypeImageCommandValidator : AbstractValidator<UpdateRoomTypeImageCommand>
{
    public UpdateRoomTypeImageCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Image ID is required.");

        RuleFor(x => x.HotelRoomTypeId)
            .NotEmpty().WithMessage("Room type ID is required.");

        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Image URL is required.")
            .MaximumLength(1000).WithMessage("Image URL must not exceed 1000 characters.");
    }
}
