using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Cities;
using MediatR;

namespace HotelBookingSystem.Application.Cities.Commands.CreateCity;

public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, Guid>
{
    private readonly IGenericRepository<City> _cityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCityCommandHandler(
        IGenericRepository<City> cityRepository,
        IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        var exists = await _cityRepository.FindAsync(c => c.CityName.ToLower() == request.CityName.ToLower()
                                                          && c.CountryName.ToLower() == request.CountryName.ToLower());

        if (exists.Count > 0)
        {
            throw new DuplicateRecordException("City already exists in this country");
        }

        var city = new City
        {
            Id = Guid.NewGuid(),
            CityName = request.CityName,
            CountryName = request.CountryName,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        await _cityRepository.AddAsync(city);
        await _unitOfWork.SaveChangesAsync();

        return city.Id;
    }
}
