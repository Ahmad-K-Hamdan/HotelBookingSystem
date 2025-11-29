using FluentValidation;

namespace HotelBookingSystem.Application.Features.HotelRoomTypes.Commands.DeleteHotelRoomType;

public class DeleteHotelRoomTypeCommandValidator : AbstractValidator<DeleteHotelRoomTypeCommand>
{
    public DeleteHotelRoomTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Room type ID is required.");
    }
}
