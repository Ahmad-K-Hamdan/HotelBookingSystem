using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Features.Hotels.Queries.GetHotelById.Dtos;
using HotelBookingSystem.Domain.Entities.Hotels;
using HotelBookingSystem.Domain.Entities.Rooms;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Hotels.Queries.GetHotelById;

public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, HotelDetailsDto>
{
    private readonly IGenericRepository<Hotel> _hotelRepository;

    public GetHotelByIdQueryHandler(IGenericRepository<Hotel> hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public async Task<HotelDetailsDto> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
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
                    .ThenInclude(r => r.Bookings)
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
                    ImageUrls = rt.Rooms
                        .SelectMany(r => r.Images)
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

        return dto;
    }

    private static bool RoomIsFree(HotelRoom room, DateOnly checkIn, DateOnly checkOut)
        => !room.Bookings.Any(b => b.CheckInDate < checkOut && b.CheckOutDate > checkIn);
}
