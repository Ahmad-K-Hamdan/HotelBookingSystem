using FluentValidation;

namespace HotelBookingSystem.Application.Features.Reviews.Queries.GetReviewById;

public class GetReviewByIdQueryValidator : AbstractValidator<GetReviewByIdQuery>
{
    public GetReviewByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Review ID is required.");
    }
}
