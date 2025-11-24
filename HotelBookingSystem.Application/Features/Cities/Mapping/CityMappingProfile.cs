using AutoMapper;
using HotelBookingSystem.Application.Features.Cities.Queries.GetCityById;
using HotelBookingSystem.Domain.Entities.Cities;

namespace HotelBookingSystem.Application.Features.Cities.Mapping;

public class CityMappingProfile : Profile
{
    public CityMappingProfile()
    {
        CreateMap<City, CityDetailsDto>();
    }
}
