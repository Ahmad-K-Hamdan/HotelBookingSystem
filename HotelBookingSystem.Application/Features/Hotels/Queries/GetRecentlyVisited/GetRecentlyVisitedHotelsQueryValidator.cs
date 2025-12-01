using FluentValidation;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetRecentlyVisited;

public class GetRecentlyVisitedHotelsQueryValidator : AbstractValidator<GetRecentlyVisitedHotelsQuery>
{
    public GetRecentlyVisitedHotelsQueryValidator()
    {
        RuleFor(x => x.Limit)
            .GreaterThan(0)
            .LessThanOrEqualTo(20)
            .WithMessage("Limit must be between 1 and 20.");
    }
}
