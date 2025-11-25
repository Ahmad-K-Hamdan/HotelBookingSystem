using MediatR;

namespace HotelBookingSystem.Application.Features.PaymentMethods.Commands.CreatePaymentMethod;

public record CreatePaymentMethodCommand(string MethodName) : IRequest<Guid>;
