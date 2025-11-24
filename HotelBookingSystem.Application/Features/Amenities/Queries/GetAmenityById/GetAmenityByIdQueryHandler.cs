using AutoMapper;
using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Amenities;
using MediatR;

namespace HotelBookingSystem.Application.Features.Amenities.Queries.GetAmenityById;

public class GetAmenityByIdQueryHandler : IRequestHandler<GetAmenityByIdQuery, AmenityDetailsDto>
{
    private readonly IGenericRepository<Amenity> _amenityRepository;
    private readonly IMapper _mapper;

    public GetAmenityByIdQueryHandler(IGenericRepository<Amenity> amenityRepository, IMapper mapper)
    {
        _amenityRepository = amenityRepository;
        _mapper = mapper;
    }

    public async Task<AmenityDetailsDto> Handle(GetAmenityByIdQuery request, CancellationToken cancellationToken)
    {
        var amenity = await _amenityRepository.GetByIdAsync(request.Id);

        if (amenity == null)
        {
            throw new NotFoundException(nameof(amenity), request.Id);
        }

        return _mapper.Map<AmenityDetailsDto>(amenity);
    }
}
