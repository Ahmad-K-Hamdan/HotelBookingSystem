using MediatR;

namespace HotelBookingSystem.Application.Features.Discounts.Commands.CreateDiscount;

public record CreateDiscountCommand(
    string DiscountDescription,
    decimal DiscountRate,
    bool IsActive) : IRequest<Guid>;
