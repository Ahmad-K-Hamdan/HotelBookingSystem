using FluentValidation;

namespace HotelBookingSystem.Application.Features.HotelRooms.Commands.DeleteHotelRoom
{
    public class DeleteHotelRoomTypeCommandValidator : AbstractValidator<DeleteHotelRoomCommand>
    {
        public DeleteHotelRoomTypeCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Room ID is required.");
        }
    }
}
