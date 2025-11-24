using MediatR;

namespace HotelBookingSystem.Application.Features.Discounts.Queries.GetDiscounts;

public record GetDiscountsQuery() : IRequest<List<DiscountDto>>;
