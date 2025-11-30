using FluentValidation;

namespace HotelBookingSystem.Application.Features.HotelRooms.Commands.CreateHotelRoom;

public class CreateHotelRoomCommandValidator : AbstractValidator<CreateHotelRoomCommand>
{
    public CreateHotelRoomCommandValidator()
    {
        RuleFor(x => x.HotelRoomTypeId)
            .NotEmpty().WithMessage("Hotel room type ID is required.");

        RuleFor(x => x.RoomNumber)
            .GreaterThan(0).WithMessage("Room number must be greater than 0.");
    }
}
