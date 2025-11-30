using MediatR;

namespace HotelBookingSystem.Application.Features.HotelImages.Commands.DeleteHotelImage;

public record DeleteHotelImageCommand(Guid Id) : IRequest<Unit>;
