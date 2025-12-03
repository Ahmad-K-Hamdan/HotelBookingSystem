using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotelDetailsById.Dtos;
using HotelBookingSystem.Domain.Entities.Hotels;
using HotelBookingSystem.Domain.Entities.Rooms;
using HotelBookingSystem.Domain.Entities.Visits;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotelDetailsById;

public class GetHotelDetailsByIdQueryHandler : IRequestHandler<GetHotelDetailsByIdQuery, HotelDetailsDto>
{
    private readonly IGenericRepository<Hotel> _hotelRepository;
    private readonly IGenericRepository<VisitLog> _visitLogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public GetHotelDetailsByIdQueryHandler(
        IGenericRepository<Hotel> hotelRepository,
        IGenericRepository<VisitLog> visitLogRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _hotelRepository = hotelRepository;
        _visitLogRepository = visitLogRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<HotelDetailsDto> Handle(GetHotelDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        var hotelQuery = _hotelRepository.Query()
            .Include(h => h.City)
            .Include(h => h.HotelGroup)
            .Include(h => h.Discount)
            .Include(h => h.Images)
            .Include(h => h.HotelAmenities)
                .ThenInclude(ha => ha.Amenity)
            .Include(h => h.RoomTypes)
                .ThenInclude(rt => rt.Rooms)
                    .ThenInclude(r => r.BookingRooms)
                        .ThenInclude(br => br.Booking)
            .Include(h => h.RoomTypes)
                .ThenInclude(rt => rt.Images)
            .Include(h => h.Reviews);

        var hotel = await hotelQuery.FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);

        if (hotel is null)
        {
            throw new NotFoundException(nameof(Hotel), request.Id);
        }

        var hasDiscount = hotel.Discount != null && hotel.Discount.IsActive;
        var discountRate = hasDiscount ? hotel.Discount!.DiscountRate : 0m;
        var discountFactor = hasDiscount ? 1 - hotel.Discount!.DiscountRate : 1m;

        var roomTypes = hotel.RoomTypes
            .Select(rt =>
            {
                var hasAvailableRoom = rt.Rooms
                    .Any(room =>
                        room.IsAvailable &&
                        RoomIsFree(room, request.CheckInDate, request.CheckOutDate));

                var originalPrice = rt.PricePerNight;
                var discountedPrice = originalPrice * discountFactor;

                return new RoomTypeDetailDto
                {
                    Id = rt.Id,
                    Name = rt.Name,
                    Description = rt.Description,
                    BedsCount = rt.BedsCount,
                    MaxAdults = rt.MaxNumOfGuestsAdults,
                    MaxChildren = rt.MaxNumOfGuestsChildren,
                    IsAvailable = hasAvailableRoom,
                    OriginalPricePerNight = originalPrice,
                    DiscountedPricePerNight = discountedPrice,
                    ImageUrls = rt.Images
                        .Where(img => !string.IsNullOrEmpty(img.Url))
                        .Select(img => img.Url)
                        .Distinct()
                        .ToList()
                };
            })
            .ToList();

        var reviews = hotel.Reviews
            .OrderByDescending(r => r.ReviewDate)
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                Rating = r.Rating,
                Comment = r.Comment!,
                ReviewDate = r.ReviewDate,
                GuestName = "Guest"
            })
            .ToList();

        double? averageRating = reviews.Count > 0 ? reviews.Average(r => r.Rating) : null;

        var dto = new HotelDetailsDto
        {
            Id = hotel.Id,
            HotelName = hotel.HotelName,
            HotelGroup = hotel.HotelGroup.GroupName,
            HotelAddress = hotel.HotelAddress,
            CityName = hotel.City.CityName,
            CountryName = hotel.City.CountryName,
            StarRating = hotel.StarRating,
            Description = hotel.Description,
            HasActiveDiscount = hasDiscount,
            DiscountRate = hasDiscount ? discountRate : null,
            ImageUrls = hotel.Images
                .OrderByDescending(i => i.IsMain)
                .ThenBy(i => i.Id)
                .Select(i => i.Url)
                .ToList(),
            Amenities = hotel.HotelAmenities
                .Select(ha => ha.Amenity.Name)
                .Distinct()
                .ToList(),
            RoomTypes = roomTypes,
            Reviews = reviews,
            AverageRating = averageRating,
            ReviewCount = reviews.Count
        };

        var userId = _currentUserService.UserId;
        if (!string.IsNullOrWhiteSpace(userId))
        {
            var visit = new VisitLog
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                HotelId = hotel.Id,
                VisitedAt = DateTime.UtcNow
            };

            await _visitLogRepository.AddAsync(visit);
            await _unitOfWork.SaveChangesAsync();
        }

        return dto;
    }

    private static bool RoomIsFree(HotelRoom room, DateOnly checkIn, DateOnly checkOut)
        => !room.BookingRooms.Any(br => br.Booking != null
                                        && br.Booking.CheckInDate < checkOut
                                        && br.Booking.CheckOutDate > checkIn);
}
