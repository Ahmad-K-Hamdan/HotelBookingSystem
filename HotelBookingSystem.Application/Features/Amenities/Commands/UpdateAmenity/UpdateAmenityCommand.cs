using MediatR;

namespace HotelBookingSystem.Application.Features.Amenities.Commands.UpdateAmenity;

public record UpdateAmenityCommand(Guid Id, string Name, string? Description) : IRequest<Unit>;
