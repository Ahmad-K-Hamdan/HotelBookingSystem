using MediatR;

namespace HotelBookingSystem.Application.Features.HotelImages.Commands.UpdateHotelImage;

public record UpdateHotelImageCommand(Guid Id, Guid HotelId, string Url, bool IsMain) : IRequest<Unit>;
