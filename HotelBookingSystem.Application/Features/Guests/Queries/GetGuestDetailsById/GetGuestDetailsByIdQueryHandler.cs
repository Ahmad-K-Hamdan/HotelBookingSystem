using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Features.Guests.Queries.GetGuestDetailsById.Dtos;
using HotelBookingSystem.Domain.Entities.Guests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Guests.Queries.GetGuestDetailsById;

public class GetGuestDetailsByIdQueryHandler : IRequestHandler<GetGuestDetailsByIdQuery, GuestDetailsDto>
{
    private readonly IGenericRepository<Guest> _guestRepository;

    public GetGuestDetailsByIdQueryHandler(IGenericRepository<Guest> guestRepository)
    {
        _guestRepository = guestRepository;
    }

    public async Task<GuestDetailsDto> Handle(GetGuestDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        var guest = await _guestRepository.Query()
            .Include(g => g.Bookings)
                .ThenInclude(b => b.Hotel)
            .Include(g => g.Reviews)
                .ThenInclude(r => r.Hotel)
            .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

        if (guest == null)
        {
            throw new NotFoundException(nameof(Guest), request.Id);
        }

        return new GuestDetailsDto
        {
            Id = guest.Id,
            UserId = guest.UserId,
            PassportNumber = guest.PassportNumber,
            HomeCountry = guest.HomeCountry,

            Reviews = guest.Reviews
                .Select(r => new GuestReviewDto
                {
                    ReviewId = r.Id,
                    HotelId = r.HotelId,
                    HotelName = r.Hotel.HotelName,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate
                })
                .ToList(),

            Bookings = guest.Bookings
                .Select(b => new GuestBookingDto
                {
                    BookingId = b.Id,
                    HotelId = b.HotelId,
                    HotelName = b.Hotel.HotelName,
                    CheckInDate = b.CheckInDate,
                    CheckOutDate = b.CheckOutDate,
                    NumOfAdults = b.TotalAdults,
                    NumOfChildren = b.TotalChildren,
                    SpecialRequests = b.SpecialRequests,
                    ConfirmationCode = b.ConfirmationCode,
                    CreatedAt = b.CreatedAt
                })
                .ToList()
        };
    }
}
