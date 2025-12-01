using MediatR;

namespace HotelBookingSystem.Application.Features.Payments.Queries.GetPaymentsForBooking;

public record GetPaymentsForBookingQuery(Guid BookingId) : IRequest<List<PaymentDto>>;
