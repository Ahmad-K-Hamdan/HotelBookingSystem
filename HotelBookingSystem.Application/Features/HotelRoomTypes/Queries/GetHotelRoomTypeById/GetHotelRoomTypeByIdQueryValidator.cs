using FluentValidation;

namespace HotelBookingSystem.Application.Features.HotelRoomTypes.Queries.GetHotelRoomTypeById;

public class GetHotelRoomTypeByIdQueryValidator : AbstractValidator<GetHotelRoomTypeByIdQuery>
{
    public GetHotelRoomTypeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Room type ID is required.");
    }
}
