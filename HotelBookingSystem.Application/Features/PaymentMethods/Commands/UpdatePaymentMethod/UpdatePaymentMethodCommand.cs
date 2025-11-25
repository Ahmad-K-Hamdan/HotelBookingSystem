using MediatR;

namespace HotelBookingSystem.Application.Features.PaymentMethods.Commands.UpdatePaymentMethod;

public record UpdatePaymentMethodCommand(Guid Id, string MethodName) : IRequest<Unit>;
