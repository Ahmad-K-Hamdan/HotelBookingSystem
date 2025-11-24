using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Amenities;
using MediatR;

namespace HotelBookingSystem.Application.Features.Amenities.Commands.DeleteAmenity;

public class DeleteAmenityCommandHandler : IRequestHandler<DeleteAmenityCommand, Unit>
{
    private readonly IGenericRepository<Amenity> _amenityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAmenityCommandHandler(IGenericRepository<Amenity> amenityRepository, IUnitOfWork unitOfWork)
    {
        _amenityRepository = amenityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteAmenityCommand request, CancellationToken cancellationToken)
    {
        var amenity = await _amenityRepository.GetByIdAsync(request.Id);

        if (amenity == null)
        {
            throw new NotFoundException(nameof(amenity), request.Id);
        }

        _amenityRepository.Delete(amenity);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}