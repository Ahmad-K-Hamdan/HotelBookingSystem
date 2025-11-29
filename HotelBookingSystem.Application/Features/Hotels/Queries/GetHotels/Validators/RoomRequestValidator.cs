using FluentValidation;
using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels.Dtos;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels.Validators;

public class RoomRequestValidator : AbstractValidator<RoomRequest>
{
    public RoomRequestValidator()
    {
        RuleFor(r => r.Adults)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Each room must have at least one adult.")
            .LessThanOrEqualTo(10)
            .WithMessage("Adults per room cannot exceed 10.");

        RuleFor(r => r.Children)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Children per room cannot be negative.")
            .LessThanOrEqualTo(10)
            .WithMessage("Children per room cannot exceed 10.");

        RuleFor(r => r.Adults + r.Children)
            .GreaterThan(0)
            .WithMessage("Total guests per room must be greater than 0.");
    }
}
