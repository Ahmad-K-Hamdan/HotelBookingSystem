using FluentValidation;

namespace HotelBookingSystem.Application.Features.Bookings.Commands.CreateBooking;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.CheckInDate)
            .NotEmpty()
            .WithMessage("Check-in date is required.");

        RuleFor(x => x.CheckOutDate)
            .NotEmpty()
            .WithMessage("Check-out date is required.")
            .GreaterThan(x => x.CheckInDate)
            .WithMessage("Check-out date must be after check-in date.");

        RuleFor(x => x.Rooms)
            .NotNull().WithMessage("At least one room is required.")
            .Must(r => r.Count > 0).WithMessage("At least one room is required.");

        RuleForEach(x => x.Rooms).ChildRules(room =>
        {
            room.RuleFor(r => r.HotelRoomTypeId)
                .NotEmpty().WithMessage("Hotel room type ID is required.");

            room.RuleFor(r => r.Adults)
                .GreaterThan(0)
                .WithMessage("Each room must have at least one adult.");

            room.RuleFor(r => r.Children)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Children count cannot be negative.");
        });

        RuleFor(x => x.SpecialRequests)
            .MaximumLength(500)
            .WithMessage("Special requests must not exceed 500 characters.");
    }
}
