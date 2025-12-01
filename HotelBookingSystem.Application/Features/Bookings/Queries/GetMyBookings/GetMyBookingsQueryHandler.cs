using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Bookings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Bookings.Queries.GetMyBookings;

public class GetMyBookingsQueryHandler : IRequestHandler<GetMyBookingsQuery, List<BookingDto>>
{
    private readonly IGenericRepository<Booking> _bookingRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetMyBookingsQueryHandler(IGenericRepository<Booking> bookingRepository, ICurrentUserService currentUserService)
    {
        _bookingRepository = bookingRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<BookingDto>> Handle(GetMyBookingsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new UnauthorizedAccessException("User not authenticated.");
        }

        var query = _bookingRepository.Query()
            .Include(b => b.Guest)
            .Include(b => b.Hotel)
                .ThenInclude(h => h.City)
            .Where(b => b.Guest.UserId == userId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        if (request.UpcomingOnly && !request.PastOnly)
        {
            query = query.Where(b => b.CheckInDate >= today);
        }
        else if (request.PastOnly && !request.UpcomingOnly)
        {
            query = query.Where(b => b.CheckOutDate < today);
        }

        var list = await query
            .OrderByDescending(b => b.CheckInDate)
            .ToListAsync(cancellationToken);

        return list.Select(b => new BookingDto
        {
            Id = b.Id,
            HotelName = b.Hotel.HotelName,
            CityName = b.Hotel.City.CityName,
            CheckInDate = b.CheckInDate,
            CheckOutDate = b.CheckOutDate,
            Nights = b.Nights,
            ConfirmationCode = b.ConfirmationCode,
            TotalOriginalPrice = b.TotalOriginalPrice,
            TotalDiscountedPrice = b.TotalDiscountedPrice
        }).ToList();
    }
}
