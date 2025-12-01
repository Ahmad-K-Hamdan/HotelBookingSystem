using FluentValidation;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotelDetailsById;

public class GetHotelDetailsByIdQueryValidator : AbstractValidator<GetHotelDetailsByIdQuery>
{
    public GetHotelDetailsByIdQueryValidator()
    {
        RuleFor(x => x.CheckInDate)
            .NotEmpty().WithMessage("Check-in date is required.")
            .LessThan(x => x.CheckOutDate)
            .WithMessage("Check-in date must be before check-out date.")
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Check-in date cannot be in the past.");

        RuleFor(x => x.CheckOutDate)
            .NotEmpty().WithMessage("Check-out date is required.");
    }
}
