using FluentValidation;

namespace HotelBookingSystem.Application.Features.RoomTypeImages.Commands.CreateRoomTypeImage;

public class CreateRoomTypeImageCommandValidator : AbstractValidator<CreateRoomTypeImageCommand>
{
    public CreateRoomTypeImageCommandValidator()
    {
        RuleFor(x => x.HotelRoomTypeId)
            .NotEmpty().WithMessage("Room type ID is required.");

        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Image URL is required.")
            .MaximumLength(1000).WithMessage("Image URL must not exceed 1000 characters.");
    }
}
