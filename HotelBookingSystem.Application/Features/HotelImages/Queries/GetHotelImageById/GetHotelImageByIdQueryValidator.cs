using FluentValidation;

namespace HotelBookingSystem.Application.Features.HotelImages.Queries.GetHotelImageById;

public class GetHotelImageByIdQueryValidator : AbstractValidator<GetHotelImageByIdQuery>
{
    public GetHotelImageByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Image ID is required.");
    }
}
