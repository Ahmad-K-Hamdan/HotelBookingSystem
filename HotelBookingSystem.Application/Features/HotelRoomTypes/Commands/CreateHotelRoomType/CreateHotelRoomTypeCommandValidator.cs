using FluentValidation;

namespace HotelBookingSystem.Application.Features.HotelRoomTypes.Commands.CreateHotelRoomType;

public class CreateHotelRoomTypeCommandValidator : AbstractValidator<CreateHotelRoomTypeCommand>
{
    public CreateHotelRoomTypeCommandValidator()
    {
        RuleFor(x => x.HotelRoomType.HotelId)
            .NotEmpty().WithMessage("Hotel ID is required.");

        RuleFor(x => x.HotelRoomType.Name)
            .NotEmpty().WithMessage("Room type name is required.")
            .MaximumLength(200).WithMessage("Room type name must not exceed 200 characters.")
            .Matches("^[A-Za-z0-9 !?,._'\"()\\-/]+$")
            .WithMessage("Room type name contains illegal characters.")
            .Must(x => x.Trim() == x)
            .WithMessage("Room type name cannot start or end with spaces.");

        RuleFor(x => x.HotelRoomType.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.")
            .Must(x => x == null || x.Trim() == x)
            .WithMessage("Description cannot start or end with spaces.");

        RuleFor(x => x.HotelRoomType.PricePerNight)
            .GreaterThan(0).WithMessage("Price per night must be greater than 0.");

        RuleFor(x => x.HotelRoomType.BedsCount)
            .GreaterThanOrEqualTo(1).WithMessage("Beds count must be at least 1.")
            .LessThanOrEqualTo(20).WithMessage("Beds count must be at most 20.");

        RuleFor(x => x.HotelRoomType.MaxNumOfGuestsAdults)
            .GreaterThanOrEqualTo(1).WithMessage("Maximum number of adult guests must be at least 1.")
            .LessThanOrEqualTo(10).WithMessage("Maximum number of adult guests must be at most 10.");

        RuleFor(x => x.HotelRoomType.MaxNumOfGuestsChildren)
            .GreaterThanOrEqualTo(0).WithMessage("Maximum number of child guests cannot be negative.")
            .LessThanOrEqualTo(10).WithMessage("Maximum number of adult guests must be at most 10.");
    }
}
