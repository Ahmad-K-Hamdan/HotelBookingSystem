using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Hotels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.HotelGroups.Queries.GetHotelGroups;

public class GetHotelGroupsQueryHandler : IRequestHandler<GetHotelGroupsQuery, List<HotelGroupDto>>
{
    private readonly IGenericRepository<HotelGroup> _hotelGroupRepository;

    public GetHotelGroupsQueryHandler(IGenericRepository<HotelGroup> hotelGroupRepository)
    {
        _hotelGroupRepository = hotelGroupRepository;
    }

    public async Task<List<HotelGroupDto>> Handle(GetHotelGroupsQuery request, CancellationToken cancellationToken)
    {
        return await _hotelGroupRepository
            .Query()
            .OrderBy(c => c)
            .Select(c => new HotelGroupDto
            {
                Id = c.Id,
                GroupName = c.GroupName,
                Description = c.Description
            })
            .ToListAsync(cancellationToken);
    }
}
