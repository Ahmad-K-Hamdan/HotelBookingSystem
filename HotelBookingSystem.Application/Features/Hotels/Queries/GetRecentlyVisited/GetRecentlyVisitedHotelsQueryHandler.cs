using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Visits;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetRecentlyVisited;

public class GetRecentlyVisitedHotelsQueryHandler : IRequestHandler<GetRecentlyVisitedHotelsQuery, List<RecentHotelDto>>
{
    private readonly IGenericRepository<VisitLog> _visitLogRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetRecentlyVisitedHotelsQueryHandler(IGenericRepository<VisitLog> visitLogRepository, ICurrentUserService currentUserService)
    {
        _visitLogRepository = visitLogRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<RecentHotelDto>> Handle(GetRecentlyVisitedHotelsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new UnauthorizedAccessException("User not authenticated.");
        }

        var logs = await _visitLogRepository.Query()
            .Where(v => v.UserId == userId)
            .Include(v => v.Hotel)
                .ThenInclude(h => h.City)
            .Include(v => v.Hotel)
                .ThenInclude(h => h.Discount)
            .Include(v => v.Hotel)
                .ThenInclude(h => h.RoomTypes)
            .Include(v => v.Hotel)
                .ThenInclude(h => h.Images)
            .OrderByDescending(v => v.VisitedAt)
            .ToListAsync(cancellationToken);

        var grouped = logs
            .GroupBy(v => v.HotelId)
            .Select(g => g.OrderByDescending(v => v.VisitedAt).First())
            .OrderByDescending(v => v.VisitedAt)
            .Take(request.Limit)
            .ToList();

        var result = grouped.Select(v =>
        {
            var hotel = v.Hotel;
            var hasDiscount = hotel.Discount != null && hotel.Discount.IsActive;
            var discountFactor = hasDiscount ? 1 - hotel.Discount!.DiscountRate : 1m;

            decimal? minOriginal = hotel.RoomTypes.Count != 0 ? hotel.RoomTypes.Min(rt => rt.PricePerNight) : null;
            decimal? minDiscounted = minOriginal.HasValue ? minOriginal.Value * discountFactor : null;

            return new RecentHotelDto
            {
                HotelId = hotel.Id,
                HotelName = hotel.HotelName,
                CityName = hotel.City.CityName,
                CountryName = hotel.City.CountryName,
                StarRating = hotel.StarRating,
                MainImageUrl = hotel.Images
                    .Where(i => i.IsMain)
                    .Select(i => i.Url)
                    .FirstOrDefault(),
                MinOriginalPricePerNight = minOriginal,
                MinDiscountedPricePerNight = minDiscounted,
                LastVisitedAt = v.VisitedAt
            };
        }).ToList();

        return result;
    }
}
