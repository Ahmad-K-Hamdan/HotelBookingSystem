using AutoMapper;
using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Discounts;
using MediatR;

namespace HotelBookingSystem.Application.Features.Discounts.Queries.GetDiscountById;

public class GetDiscountByIdQueryHandler : IRequestHandler<GetDiscountByIdQuery, DiscountDetailsDto>
{
    private readonly IGenericRepository<Discount> _discountRepository;
    private readonly IMapper _mapper;

    public GetDiscountByIdQueryHandler(IGenericRepository<Discount> discountRepository, IMapper mapper)
    {
        _discountRepository = discountRepository;
        _mapper = mapper;
    }

    public async Task<DiscountDetailsDto> Handle(GetDiscountByIdQuery request, CancellationToken cancellationToken)
    {
        var discount = await _discountRepository.GetByIdAsync(request.Id);

        if (discount == null)
        {
            throw new NotFoundException(nameof(discount), request.Id);
        }

        return _mapper.Map<DiscountDetailsDto>(discount);
    }
}
