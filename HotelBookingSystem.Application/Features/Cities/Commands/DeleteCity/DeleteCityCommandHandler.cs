using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Cities;
using MediatR;

namespace HotelBookingSystem.Application.Features.Cities.Commands.DeleteCity;

public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, Unit>
{
    private readonly IGenericRepository<City> _cityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCityCommandHandler(IGenericRepository<City> cityRepository, IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(request.Id);

        if (city == null)
        {
            throw new NotFoundException(nameof(city), request.Id);
        }

        _cityRepository.Delete(city);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}