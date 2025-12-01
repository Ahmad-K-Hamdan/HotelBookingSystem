using FluentValidation;
using FluentValidation.Results;
using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Bookings;
using HotelBookingSystem.Domain.Entities.Payments;
using HotelBookingSystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Payments.Commands.CreatePaymentForBooking;

public class CreatePaymentForBookingCommandHandler : IRequestHandler<CreatePaymentForBookingCommand, Guid>
{
    private readonly IGenericRepository<Booking> _bookingRepository;
    private readonly IGenericRepository<Payment> _paymentRepository;
    private readonly IGenericRepository<PaymentMethod> _paymentMethodRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePaymentForBookingCommandHandler(
        IGenericRepository<Booking> bookingRepository,
        IGenericRepository<Payment> paymentRepository,
        IGenericRepository<PaymentMethod> paymentMethodRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _paymentRepository = paymentRepository;
        _paymentMethodRepository = paymentMethodRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreatePaymentForBookingCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new UnauthorizedAccessException("User not authenticated.");
        }

        var booking = await _bookingRepository.Query()
            .Include(b => b.Guest)
            .Include(b => b.Payments)
            .Include(b => b.Hotel)
            .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

        if (booking is null)
        {
            throw new NotFoundException(nameof(Booking), request.BookingId);
        }

        if (booking.Guest.UserId != userId)
        {
            throw new UnauthorizedAccessException("You are not allowed to pay for this booking.");
        }

        _ = await _paymentMethodRepository.GetByIdAsync(request.PaymentMethodId)
            ?? throw new NotFoundException("Payment method", request.PaymentMethodId);

        var alreadyPaid = booking.Payments
            .Where(p => p.PaymentStatus == PaymentStatus.Completed)
            .Sum(p => p.PaymentAmount);

        var remaining = booking.TotalDiscountedPrice - alreadyPaid;

        if (remaining <= 0)
        {
            throw new ValidationException(
            [
                new ValidationFailure("BookingId", "Booking is already fully paid. No remaining balance to pay.")
            ]);
        }

        if (request.Amount > remaining)
        {
            throw new ValidationException(
            [
                new ValidationFailure("Amount", $"Payment amount ({request.Amount}) exceeds remaining balance ({remaining}).")
            ]);
        }

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            BookingId = booking.Id,
            PaymentMethodId = request.PaymentMethodId,
            DiscountId = booking.Hotel.DiscountId,
            PaymentAmount = request.Amount,
            PaymentStatus = PaymentStatus.Completed,
            PaymentDate = DateTime.UtcNow
        };

        await _paymentRepository.AddAsync(payment);
        await _unitOfWork.SaveChangesAsync();
        return payment.Id;
    }
}
