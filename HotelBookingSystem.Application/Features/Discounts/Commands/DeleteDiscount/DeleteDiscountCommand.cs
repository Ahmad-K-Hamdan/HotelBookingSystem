using MediatR;

namespace HotelBookingSystem.Application.Features.Discounts.Commands.DeleteDiscount;

public record DeleteDiscountCommand(Guid Id) : IRequest<Unit>;
