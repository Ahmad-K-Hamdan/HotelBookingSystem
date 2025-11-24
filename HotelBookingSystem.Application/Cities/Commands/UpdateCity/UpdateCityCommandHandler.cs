using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Cities;
using MediatR;

namespace HotelBookingSystem.Application.Cities.Commands.UpdateCity;

public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand, Unit>
{
    private readonly IGenericRepository<City> _cityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCityCommandHandler(IGenericRepository<City> cityRepository, IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(request.Id);

        if (city == null)
        {
            throw new NotFoundException(nameof(city), request.Id);
        }

        city.CityName = request.CityName;
        city.CountryName = request.CountryName;
        city.Description = request.Description;
        city.UpdatedAt = DateTime.UtcNow;

        _cityRepository.Update(city);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}