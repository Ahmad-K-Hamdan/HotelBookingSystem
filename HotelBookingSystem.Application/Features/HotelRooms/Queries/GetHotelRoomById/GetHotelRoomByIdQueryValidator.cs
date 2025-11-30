using FluentValidation;

namespace HotelBookingSystem.Application.Features.HotelRooms.Queries.GetHotelRoomById;

public class GetHotelRoomByIdQueryValidator : AbstractValidator<GetHotelRoomByIdQuery>
{
    public GetHotelRoomByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Room ID is required.");
    }
}
