using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Amenities;
using MediatR;

namespace HotelBookingSystem.Application.Features.Amenities.Commands.CreateAmenity;

public class CreateAmenityCommandHandler : IRequestHandler<CreateAmenityCommand, Guid>
{
    private readonly IGenericRepository<Amenity> _amenityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAmenityCommandHandler(
        IGenericRepository<Amenity> amenityRepository,
        IUnitOfWork unitOfWork)
    {
        _amenityRepository = amenityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateAmenityCommand request, CancellationToken cancellationToken)
    {
        var exists = await _amenityRepository.FindAsync(c => c.Name.ToLower() == request.Name.ToLower());

        if (exists.Count > 0)
        {
            throw new DuplicateRecordException("Amenity already exists.");
        }

        var amenity = new Amenity
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
        };

        await _amenityRepository.AddAsync(amenity);
        await _unitOfWork.SaveChangesAsync();

        return amenity.Id;
    }
}
