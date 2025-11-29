using FluentValidation;

namespace HotelBookingSystem.Application.Features.Guests.Queries.GetGuestById;

public class GetGuestByIdQueryValidator : AbstractValidator<GetGuestByIdQuery>
{
    public GetGuestByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Guest ID is required.");
    }
}
