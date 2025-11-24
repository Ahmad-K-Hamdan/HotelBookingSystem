using AutoMapper;
using HotelBookingSystem.Application.Features.Amenities.Queries.GetAmenityById;
using HotelBookingSystem.Domain.Entities.Amenities;

namespace HotelBookingSystem.Application.Features.Amenities.Mapping;

public class AmenityMappingProfile : Profile
{
    public AmenityMappingProfile()
    {
        CreateMap<Amenity, AmenityDetailsDto>();
    }
}
