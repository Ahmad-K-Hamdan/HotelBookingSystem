using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels;
using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotels.Dtos;
using HotelBookingSystem.Domain.Entities.Hotels;
using HotelBookingSystem.Domain.Entities.Rooms;
using HotelBookingSystem.Domain.Entities.Visits;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Hotels.GetHotels;

public partial class GetHotelsQueryHandler : IRequestHandler<GetHotelsQuery, List<HotelDto>>
{
    private readonly IGenericRepository<Hotel> _hotelRepository;
    private readonly IGenericRepository<VisitLog> _visitLogRepository;

    public GetHotelsQueryHandler(IGenericRepository<Hotel> hotelRepository, IGenericRepository<VisitLog> visitLogRepository)
    {
        _hotelRepository = hotelRepository;
        _visitLogRepository = visitLogRepository;
    }

    public async Task<List<HotelDto>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
    {
        var visitsQuery = _visitLogRepository.Query();

        IQueryable<Hotel> hotels = _hotelRepository.Query()
            .Include(h => h.City)
            .Include(h => h.HotelGroup)
            .Include(h => h.Discount)
            .Include(h => h.RoomTypes)
                .ThenInclude(rt => rt.Rooms)
                    .ThenInclude(r => r.BookingRooms)
                        .ThenInclude(br => br.Booking)
            .Include(h => h.Images)
            .Include(h => h.HotelAmenities)
                .ThenInclude(ha => ha.Amenity);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();

            hotels = hotels.Where(h =>
                h.HotelName.ToLower().Contains(search) ||
                h.City.CityName.ToLower().Contains(search) ||
                h.City.CountryName.ToLower().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(request.CityName))
        {
            var city = request.CityName.Trim().ToLower();
            hotels = hotels.Where(h => h.City.CityName.ToLower() == city);
        }

        if (!string.IsNullOrWhiteSpace(request.CountryName))
        {
            var country = request.CountryName.Trim().ToLower();
            hotels = hotels.Where(h => h.City.CountryName.ToLower() == country);
        }

        if (request.MinStars.HasValue)
        {
            hotels = hotels.Where(h => h.StarRating >= request.MinStars.Value);
        }

        if (request.OnlyWithActiveDiscount)
        {
            hotels = hotels.Where(h => h.Discount != null && h.Discount.IsActive);
        }

        if (request.HotelGroupId.HasValue)
        {
            hotels = hotels.Where(h => h.HotelGroupId == request.HotelGroupId.Value);
        }

        if (request.AmenityIds is { Count: > 0 })
        {
            foreach (var amenityId in request.AmenityIds.Distinct())
            {
                hotels = hotels.Where(h => h.HotelAmenities.Any(ha => ha.AmenityId == amenityId));
            }
        }

        if (request.RoomTypes is { Count: > 0 })
        {
            var normalizedTypes = request.RoomTypes
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => t.Trim().ToLower())
                .ToList();

            if (normalizedTypes.Count > 0)
            {
                hotels = hotels.Where(h => h.RoomTypes.Any(rt => normalizedTypes.Contains(rt.Name.ToLower())));
            }
        }

        var hotelList = await hotels.ToListAsync(cancellationToken);

        var assignments = new List<HotelAssignment>();

        foreach (var hotel in hotelList)
        {
            List<(HotelRoom Room, HotelRoomType Type)> assignedRooms;
            var hasRequests = request.Rooms is { Count: > 0 };

            if (hasRequests)
            {
                if (!TryAssignRooms(hotel, request.Rooms, request.CheckInDate, request.CheckOutDate, out assignedRooms))
                {
                    continue;
                }
            }
            else
            {
                assignedRooms = new List<(HotelRoom Room, HotelRoomType Type)>();
            }

            var discountFactor = hotel.Discount != null && hotel.Discount.IsActive ? 1 - hotel.Discount.DiscountRate : 1m;

            decimal basePerNight;
            if (assignedRooms.Count > 0)
            {
                basePerNight = assignedRooms.Sum(x => x.Type.PricePerNight);
            }
            else
            {
                basePerNight = hotel.RoomTypes.Any() ? hotel.RoomTypes.Min(rt => rt.PricePerNight) : 0m;
            }

            var discountedPerNight = basePerNight * discountFactor;

            assignments.Add(new HotelAssignment
            {
                Hotel = hotel,
                AssignedRooms = assignedRooms,
                BasePricePerNight = basePerNight,
                DiscountedPricePerNight = discountedPerNight
            });
        }

        if (request.MinPrice.HasValue)
        {
            assignments = assignments
                .Where(a => a.DiscountedPricePerNight >= request.MinPrice.Value)
                .ToList();
        }

        if (request.MaxPrice.HasValue)
        {
            assignments = assignments
                .Where(a => a.DiscountedPricePerNight <= request.MaxPrice.Value)
                .ToList();
        }

        var dtoQuery = assignments
            .Select(a =>
            {
                var hasDiscount = a.Hotel.Discount != null && a.Hotel.Discount.IsActive;
                var discountFactor = hasDiscount ? 1 - a.Hotel.Discount!.DiscountRate : 1m;

                return new HotelDto
                {
                    Id = a.Hotel.Id,
                    HotelName = a.Hotel.HotelName,
                    HotelGroup = a.Hotel.HotelGroup.GroupName,
                    HotelAddress = a.Hotel.HotelAddress,
                    CityName = a.Hotel.City.CityName,
                    CountryName = a.Hotel.City.CountryName,
                    StarRating = a.Hotel.StarRating,
                    HasActiveDiscount = hasDiscount,
                    MinTotalOriginalPricePerNight = a.BasePricePerNight,
                    MinTotalDiscountedPricePerNight = a.DiscountedPricePerNight,
                    VisitCount = visitsQuery.Count(v => v.HotelId == a.Hotel.Id),
                    MainImageUrl = a.Hotel.Images
                        .Where(img => img.IsMain)
                        .Select(img => img.Url)
                        .FirstOrDefault(),
                    AmenityNames = a.Hotel.HotelAmenities
                        .Select(ha => ha.Amenity.Name)
                        .Distinct()
                        .ToList(),
                    MatchedRooms = a.AssignedRooms
                        .Select(x => new MatchedRoomDto
                        {
                            RoomId = x.Room.Id,
                            RoomTypeName = x.Type.Name,
                            OriginalPricePerNight = x.Type.PricePerNight,
                            DiscountedPricePerNight = x.Type.PricePerNight * discountFactor,
                            MaxAdults = x.Type.MaxNumOfGuestsAdults,
                            MaxChildren = x.Type.MaxNumOfGuestsChildren
                        })
                        .ToList()
                };
            })
            .AsQueryable();

        var finalQuery = ApplySorting(dtoQuery, request.Sort);

        if (request.Limit.HasValue && request.Limit.Value > 0)
        {
            finalQuery = finalQuery.Take(request.Limit.Value);
        }

        return finalQuery.ToList();
    }

    private static bool RoomIsFree(HotelRoom room, DateOnly checkIn, DateOnly checkOut)
        => !room.BookingRooms.Any(br =>
            br.Booking.CheckInDate < checkOut &&
            br.Booking.CheckOutDate > checkIn);

    private static bool TryAssignRooms(
        Hotel hotel,
        List<RoomRequest> requests,
        DateOnly checkIn,
        DateOnly checkOut,
        out List<(HotelRoom Room, HotelRoomType Type)> assignedRooms)
    {
        assignedRooms = new List<(HotelRoom Room, HotelRoomType Type)>();

        if (requests == null || requests.Count == 0)
        {
            return true;
        }

        var availableRooms = hotel.RoomTypes
            .SelectMany(rt => rt.Rooms.Select(r => new
            {
                Room = r,
                Type = rt
            }))
            .Where(x =>
                x.Room.IsAvailable &&
                RoomIsFree(x.Room, checkIn, checkOut))
            .OrderBy(x => x.Type.PricePerNight)
            .ToList();

        if (availableRooms.Count == 0)
        {
            return false;
        }

        var sortedRequests = requests
            .OrderByDescending(r => r.Adults + r.Children)
            .ThenByDescending(r => r.Adults)
            .ToList();

        foreach (var req in sortedRequests)
        {
            var index = availableRooms.FindIndex(ar =>
                ar.Type.MaxNumOfGuestsAdults >= req.Adults &&
                ar.Type.MaxNumOfGuestsChildren >= req.Children);

            if (index == -1)
            {
                assignedRooms.Clear();
                return false;
            }

            var candidate = availableRooms[index];
            assignedRooms.Add((candidate.Room, candidate.Type));
            availableRooms.RemoveAt(index);
        }

        return true;
    }

    private static IQueryable<HotelDto> ApplySorting(IQueryable<HotelDto> query, string? sort)
    {
        if (string.IsNullOrWhiteSpace(sort))
        {
            return query
                .OrderBy(h => h.MinTotalDiscountedPricePerNight)
                .ThenByDescending(h => h.StarRating);
        }

        var sorting = sort.Trim().ToLowerInvariant();

        return sorting switch
        {
            "price" => query
                .OrderBy(h => h.MinTotalDiscountedPricePerNight),
            "price desc" => query
                .OrderByDescending(h => h.MinTotalDiscountedPricePerNight),
            "stars" => query
                .OrderBy(h => h.StarRating),
            "stars desc" => query
                .OrderByDescending(h => h.StarRating),
            "most visited" => query
                .OrderByDescending(h => h.VisitCount),
            _ => query
                .OrderBy(h => h.MinTotalDiscountedPricePerNight)
                .ThenByDescending(h => h.StarRating)
        };
    }
}
