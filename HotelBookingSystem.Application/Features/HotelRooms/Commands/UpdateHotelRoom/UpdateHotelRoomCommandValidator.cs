using FluentValidation;

namespace HotelBookingSystem.Application.Features.HotelRooms.Commands.UpdateHotelRoom;

public class UpdateHotelRoomCommandValidator : AbstractValidator<UpdateHotelRoomCommand>
{
    public UpdateHotelRoomCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Room ID is required.");

        RuleFor(x => x.HotelRoomTypeId)
            .NotEmpty().WithMessage("Hotel room type ID is required.");

        RuleFor(x => x.RoomNumber)
            .GreaterThan(0).WithMessage("Room number must be greater than 0.");
    }
}
