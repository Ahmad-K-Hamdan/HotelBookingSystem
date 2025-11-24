using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Cities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Cities.Queries.GetCities;

public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, List<CityDto>>
{
    private readonly IGenericRepository<City> _cityRepository;

    public GetCitiesQueryHandler(IGenericRepository<City> cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<List<CityDto>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
    {
        var cities = await _cityRepository
            .Query()                   
            .OrderBy(c => c.CityName)  
            .Select(c => new CityDto
            {
                Id = c.Id,
                CityName = c.CityName,
                CountryName = c.CountryName,
                Description = c.Description
            })
            .ToListAsync(cancellationToken);

        return cities;
    }
}
