using AutoMapper;
using HotelBookingSystem.Application.Cities.Queries.GetCityById;
using HotelBookingSystem.Domain.Entities.Cities;

namespace HotelBookingSystem.Application.Cities.Mapping;

public class CityMappingProfile : Profile
{
    public CityMappingProfile()
    {
        CreateMap<City, CityDetailsDto>();
    }
}
