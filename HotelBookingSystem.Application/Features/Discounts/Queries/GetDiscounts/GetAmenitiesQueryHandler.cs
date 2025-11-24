using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Discounts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Discounts.Queries.GetDiscounts;

public class GetDiscountsQueryHandler : IRequestHandler<GetDiscountsQuery, List<DiscountDto>>
{
    private readonly IGenericRepository<Discount> _discountRepository;

    public GetDiscountsQueryHandler(IGenericRepository<Discount> discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<List<DiscountDto>> Handle(GetDiscountsQuery request, CancellationToken cancellationToken)
    {
        return await _discountRepository
            .Query()
            .OrderBy(c => c)
            .Select(c => new DiscountDto
            {
                Id = c.Id,
                DiscountDescription = c.DiscountDescription,
                DiscountRate = c.DiscountRate,
                IsActive = c.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}
