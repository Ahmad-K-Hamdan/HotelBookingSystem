using AutoMapper;
using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Hotels;
using MediatR;

namespace HotelBookingSystem.Application.Features.HotelGroups.Queries.GetHotelGroupById;

public class GetHotelGroupByIdQueryHandler : IRequestHandler<GetHotelGroupByIdQuery, HotelGroupDetailsDto>
{
    private readonly IGenericRepository<HotelGroup> _hotelGroupRepository;
    private readonly IMapper _mapper;

    public GetHotelGroupByIdQueryHandler(IGenericRepository<HotelGroup> hotelGroupRepository, IMapper mapper)
    {
        _hotelGroupRepository = hotelGroupRepository;
        _mapper = mapper;
    }

    public async Task<HotelGroupDetailsDto> Handle(GetHotelGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var hotelGroup = await _hotelGroupRepository.GetByIdAsync(request.Id);

        if (hotelGroup == null)
        {
            throw new NotFoundException(nameof(hotelGroup), request.Id);
        }

        return _mapper.Map<HotelGroupDetailsDto>(hotelGroup);
    }
}
