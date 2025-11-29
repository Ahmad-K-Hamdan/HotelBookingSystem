using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Guests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Guests.Queries.GetGuests;

public class GetGuestsQueryHandler : IRequestHandler<GetGuestsQuery, List<GuestListDto>>
{
    private readonly IGenericRepository<Guest> _guestRepository;

    public GetGuestsQueryHandler(IGenericRepository<Guest> guestRepository)
    {
        _guestRepository = guestRepository;
    }

    public async Task<List<GuestListDto>> Handle(GetGuestsQuery request, CancellationToken cancellationToken)
    {
        return await _guestRepository.Query()
            .Select(g => new GuestListDto
            {
                Id = g.Id,
                UserId = g.UserId,
                PassportNumber = g.PassportNumber,
                HomeCountry = g.HomeCountry
            })
            .ToListAsync(cancellationToken);
    }
}
