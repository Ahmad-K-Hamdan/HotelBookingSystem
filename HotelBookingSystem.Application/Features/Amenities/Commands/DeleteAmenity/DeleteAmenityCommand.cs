using MediatR;

namespace HotelBookingSystem.Application.Features.Amenities.Commands.DeleteAmenity;

public record DeleteAmenityCommand(Guid Id) : IRequest<Unit>;
