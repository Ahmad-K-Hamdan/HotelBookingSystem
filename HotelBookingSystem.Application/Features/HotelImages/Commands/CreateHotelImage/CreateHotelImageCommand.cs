using MediatR;

namespace HotelBookingSystem.Application.Features.HotelImages.Commands.CreateHotelImage;

public record CreateHotelImageCommand(Guid HotelId, string Url, bool IsMain) : IRequest<Guid>;
