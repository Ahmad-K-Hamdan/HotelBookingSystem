using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Amenities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Amenities.Queries.GetAmenities;

public class GetAmenitiesQueryHandler : IRequestHandler<GetAmenitiesQuery, List<AmenityDto>>
{
    private readonly IGenericRepository<Amenity> _amenityRepository;

    public GetAmenitiesQueryHandler(IGenericRepository<Amenity> amenityRepository)
    {
        _amenityRepository = amenityRepository;
    }

    public async Task<List<AmenityDto>> Handle(GetAmenitiesQuery request, CancellationToken cancellationToken)
    {
        return await _amenityRepository
            .Query()
            .OrderBy(c => c.Name)
            .Select(c => new AmenityDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            })
            .ToListAsync(cancellationToken);
    }
}
