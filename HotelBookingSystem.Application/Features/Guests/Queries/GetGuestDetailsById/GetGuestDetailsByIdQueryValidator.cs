using FluentValidation;

namespace HotelBookingSystem.Application.Features.Guests.Queries.GetGuestDetailsById;

public class GetGuestDetailsByIdQueryValidator : AbstractValidator<GetGuestDetailsByIdQuery>
{
    public GetGuestDetailsByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Guest ID is required.");
    }
}
