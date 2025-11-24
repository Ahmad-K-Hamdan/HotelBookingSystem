using AutoMapper;
using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Cities;
using MediatR;

namespace HotelBookingSystem.Application.Cities.Queries.GetCityById;

public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, CityDetailsDto>
{
    private readonly IGenericRepository<City> _cityRepository;
    private readonly IMapper _mapper;

    public GetCityByIdQueryHandler(IGenericRepository<City> cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    public async Task<CityDetailsDto> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(request.Id);

        if (city == null)
        {
            throw new NotFoundException(nameof(city), request.Id);
        }

        return _mapper.Map<CityDetailsDto>(city);
    }
}
