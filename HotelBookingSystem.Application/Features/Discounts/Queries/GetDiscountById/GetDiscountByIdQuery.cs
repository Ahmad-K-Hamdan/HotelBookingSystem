using MediatR;

namespace HotelBookingSystem.Application.Features.Discounts.Queries.GetDiscountById;

public record GetDiscountByIdQuery(Guid Id) : IRequest<DiscountDetailsDto>;
