using MediatR;

namespace HotelBookingSystem.Application.Features.PaymentMethods.Queries.GetPaymentMethodById;

public record GetPaymentMethodByIdQuery(Guid Id) : IRequest<PaymentMethodDetailsDto>;
