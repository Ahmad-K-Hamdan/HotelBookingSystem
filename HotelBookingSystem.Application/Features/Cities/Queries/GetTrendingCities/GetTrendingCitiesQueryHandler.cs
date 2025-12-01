using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Visits;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Cities.Queries.GetTrendingCities;

public class GetTrendingCitiesQueryHandler : IRequestHandler<GetTrendingCitiesQuery, List<TrendingCityDto>>
{
    private readonly IGenericRepository<VisitLog> _visitLogRepository;

    public GetTrendingCitiesQueryHandler(IGenericRepository<VisitLog> visitLogRepository)
    {
        _visitLogRepository = visitLogRepository;
    }

    public async Task<List<TrendingCityDto>> Handle(GetTrendingCitiesQuery request, CancellationToken cancellationToken)
    {
        var query = _visitLogRepository.Query()
            .Include(v => v.Hotel)
                .ThenInclude(h => h.City)
            .Include(v => v.Hotel)
                .ThenInclude(h => h.Images)
            .AsQueryable();

        if (request.DaysBack.HasValue)
        {
            query = query.Where(v => v.VisitedAt >= DateTime.UtcNow.AddDays(-request.DaysBack.Value));
        }

        var cityStats = await query
            .GroupBy(v => v.Hotel.CityId)
            .Select(g => new TrendingCityDto
            {
                CityId = g.Key,
                CityName = g.First().Hotel.City.CityName,
                CountryName = g.First().Hotel.City.CountryName,
                VisitCount = g.Count()
            })
            .OrderByDescending(c => c.VisitCount)
            .Take(request.Limit)
            .ToListAsync(cancellationToken);

        return cityStats;
    }
}
