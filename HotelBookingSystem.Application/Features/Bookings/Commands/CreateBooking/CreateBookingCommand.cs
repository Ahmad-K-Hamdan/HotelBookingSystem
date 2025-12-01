using MediatR;

namespace HotelBookingSystem.Application.Features.Bookings.Commands.CreateBooking;

public class CreateBookingCommand : IRequest<Guid>
{
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }

    public List<BookingRoomRequestDto> Rooms { get; set; } = new();
    public string? SpecialRequests { get; set; }
}
