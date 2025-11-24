using MediatR;

namespace HotelBookingSystem.Application.Features.Amenities.Queries.GetAmenityById;

public record GetAmenityByIdQuery(Guid Id) : IRequest<AmenityDetailsDto>;
