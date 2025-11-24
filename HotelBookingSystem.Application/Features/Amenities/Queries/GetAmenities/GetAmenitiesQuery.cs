using MediatR;

namespace HotelBookingSystem.Application.Features.Amenities.Queries.GetAmenities;

public record GetAmenitiesQuery() : IRequest<List<AmenityDto>>;
