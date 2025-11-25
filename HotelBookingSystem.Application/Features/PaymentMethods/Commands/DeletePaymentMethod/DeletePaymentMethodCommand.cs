using MediatR;

namespace HotelBookingSystem.Application.Features.PaymentMethods.Commands.DeletePaymentMethod;

public record DeletePaymentMethodCommand(Guid Id) : IRequest<Unit>;
