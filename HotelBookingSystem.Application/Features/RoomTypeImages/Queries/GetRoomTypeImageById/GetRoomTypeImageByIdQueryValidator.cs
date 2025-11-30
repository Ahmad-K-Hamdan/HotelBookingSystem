using FluentValidation;

namespace HotelBookingSystem.Application.Features.RoomTypeImages.Queries.GetRoomTypeImageById;

public class GetRoomTypeImageByIdQueryValidator : AbstractValidator<GetRoomTypeImageByIdQuery>
{
    public GetRoomTypeImageByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Image ID is required.");
    }
}
