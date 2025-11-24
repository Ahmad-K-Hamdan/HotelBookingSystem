using MediatR;

namespace HotelBookingSystem.Application.Features.Discounts.Commands.UpdateDiscount;

public record UpdateDiscountCommand(
    Guid Id,
    string DiscountDescription,
    decimal DiscountRate,
    bool IsActive) : IRequest<Unit>;
