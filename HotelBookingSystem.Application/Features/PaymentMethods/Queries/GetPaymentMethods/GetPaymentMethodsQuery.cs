using MediatR;

namespace HotelBookingSystem.Application.Features.PaymentMethods.Queries.GetPaymentMethods;

public record GetPaymentMethodsQuery() : IRequest<List<PaymentMethodDto>>;
