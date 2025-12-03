using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Features.Bookings.Queries.GetBookingDetailsById.Dtos;
using HotelBookingSystem.Domain.Entities.Bookings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Bookings.Queries.GetBookingDetailsById;

public class GetBookingDetailsByIdQueryHandler : IRequestHandler<GetBookingDetailsByIdQuery, BookingDetailsDto>
{
    private readonly IGenericRepository<Booking> _bookingRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;

    public GetBookingDetailsByIdQueryHandler(
        IGenericRepository<Booking> bookingRepository,
        ICurrentUserService currentUserService,
        IIdentityService identityService)
    {
        _bookingRepository = bookingRepository;
        _currentUserService = currentUserService;
        _identityService = identityService;
    }

    public async Task<BookingDetailsDto> Handle(GetBookingDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new UnauthorizedAccessException("User not authenticated.");
        }

        var booking = await _bookingRepository.Query()
            .Include(b => b.Guest)
            .Include(b => b.Hotel)
                .ThenInclude(h => h.City)
            .Include(b => b.BookingRooms)
                .ThenInclude(br => br.HotelRoom)
                    .ThenInclude(hr => hr.RoomType)
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

        if (booking is null)
        {
            throw new NotFoundException(nameof(Booking), request.Id);
        }

        if (booking.Guest.UserId != userId)
        {
            throw new UnauthorizedAccessException("You are not allowed to view this booking.");
        }

        var userGuest = await _identityService.GetUserByIdAsync(booking.Guest.UserId);

        var dto = new BookingDetailsDto
        {
            Id = booking.Id,
            CheckInDate = booking.CheckInDate,
            CheckOutDate = booking.CheckOutDate,
            Nights = booking.Nights,
            TotalAdults = booking.TotalAdults,
            TotalChildren = booking.TotalChildren,
            ConfirmationCode = booking.ConfirmationCode,
            SpecialRequests = booking.SpecialRequests,
            CreatedAt = booking.CreatedAt,
            TotalOriginalPrice = booking.TotalOriginalPrice,
            TotalDiscountedPrice = booking.TotalDiscountedPrice,
            HotelId = booking.HotelId,
            HotelName = booking.Hotel.HotelName,
            CityName = booking.Hotel.City.CityName,
            CountryName = booking.Hotel.City.CountryName,
            StarRating = booking.Hotel.StarRating,
            GuestId = booking.GuestId,
            GuestHomeCountry = booking.Guest.HomeCountry,
            GuestFullName = $"{userGuest!.FirstName} {userGuest!.LastName}",
            GuestPassportNumber = booking.Guest.PassportNumber,
            Rooms = booking.BookingRooms
                .Select(br => new BookingRoomDto
                {
                    BookingRoomId = br.Id,
                    HotelRoomId = br.HotelRoomId,
                    RoomTypeName = br.HotelRoom.RoomType.Name,
                    RoomNumber = br.HotelRoom.RoomNumber,
                    Adults = br.NumOfAdults,
                    Children = br.NumOfChildren,
                    PricePerNightOriginal = br.PricePerNightOriginal,
                    PricePerNightDiscounted = br.PricePerNightDiscounted
                })
                .ToList()
        };

        return dto;
    }
}
