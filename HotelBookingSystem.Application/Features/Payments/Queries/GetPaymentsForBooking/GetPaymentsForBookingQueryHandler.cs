using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Bookings;
using HotelBookingSystem.Domain.Entities.Payments;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Payments.Queries.GetPaymentsForBooking;

public class GetPaymentsForBookingQueryHandler : IRequestHandler<GetPaymentsForBookingQuery, List<PaymentDto>>
{
    private readonly IGenericRepository<Booking> _bookingRepository;
    private readonly IGenericRepository<Payment> _paymentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetPaymentsForBookingQueryHandler(
        IGenericRepository<Booking> bookingRepository,
        IGenericRepository<Payment> paymentRepository,
        ICurrentUserService currentUserService)
    {
        _bookingRepository = bookingRepository;
        _paymentRepository = paymentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<PaymentDto>> Handle(GetPaymentsForBookingQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new UnauthorizedAccessException("User not authenticated.");
        }

        var booking = await _bookingRepository.Query()
            .Include(b => b.Guest)
            .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

        if (booking is null)
        {
            throw new NotFoundException(nameof(Booking), request.BookingId);
        }

        if (booking.Guest.UserId != userId)
        {
            throw new UnauthorizedAccessException("You are not allowed to view payments for this booking.");
        }

        var payments = await _paymentRepository.Query()
            .Where(p => p.BookingId == request.BookingId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync(cancellationToken);

        return payments.Select(p => new PaymentDto
        {
            Id = p.Id,
            PaymentAmount = p.PaymentAmount,
            PaymentStatus = p.PaymentStatus,
            PaymentDate = p.PaymentDate,
            PaymentMethodId = p.PaymentMethodId
        }).ToList();
    }
}
