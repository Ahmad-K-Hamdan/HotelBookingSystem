using MediatR;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetRecentlyVisited;

public class GetRecentlyVisitedHotelsQuery : IRequest<List<RecentHotelDto>>
{
    public int Limit { get; set; } = 5;
}
