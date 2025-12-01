using FluentValidation;

namespace HotelBookingSystem.Application.Features.Cities.Queries.GetTrendingCities;

public class GetTrendingCitiesQueryValidator : AbstractValidator<GetTrendingCitiesQuery>
{
    public GetTrendingCitiesQueryValidator()
    {
        RuleFor(x => x.Limit)
            .GreaterThan(0)
            .LessThanOrEqualTo(20)
            .WithMessage("Limit must be between 1 and 20.");

        RuleFor(x => x.DaysBack)
            .GreaterThan(0)
            .When(x => x.DaysBack.HasValue)
            .WithMessage("DaysBack must be greater than 0.");
    }
}
