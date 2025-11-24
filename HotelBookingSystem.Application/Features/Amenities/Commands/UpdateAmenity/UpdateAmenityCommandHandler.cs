using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Amenities;
using MediatR;

namespace HotelBookingSystem.Application.Features.Amenities.Commands.UpdateAmenity;

public class UpdateAmenityCommandHandler : IRequestHandler<UpdateAmenityCommand, Unit>
{
    private readonly IGenericRepository<Amenity> _amenityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAmenityCommandHandler(IGenericRepository<Amenity> amenityRepository, IUnitOfWork unitOfWork)
    {
        _amenityRepository = amenityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateAmenityCommand request, CancellationToken cancellationToken)
    {
        var amenity = await _amenityRepository.GetByIdAsync(request.Id);

        if (amenity == null)
        {
            throw new NotFoundException(nameof(amenity), request.Id);
        }

        amenity.Name = request.Name;
        amenity.Description = request.Description;

        _amenityRepository.Update(amenity);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}