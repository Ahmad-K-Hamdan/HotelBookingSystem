using MediatR;

namespace HotelBookingSystem.Application.Features.Amenities.Commands.CreateAmenity;

public record CreateAmenityCommand(string Name, string? Description) : IRequest<Guid>;
