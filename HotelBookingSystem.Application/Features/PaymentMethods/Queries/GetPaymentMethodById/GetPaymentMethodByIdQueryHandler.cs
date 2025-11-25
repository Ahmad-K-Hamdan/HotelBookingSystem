using AutoMapper;
using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Payments;
using MediatR;

namespace HotelBookingSystem.Application.Features.PaymentMethods.Queries.GetPaymentMethodById;

public class GetPaymentMethodByIdQueryHandler : IRequestHandler<GetPaymentMethodByIdQuery, PaymentMethodDetailsDto>
{
    private readonly IGenericRepository<PaymentMethod> _paymentMethodRepository;
    private readonly IMapper _mapper;

    public GetPaymentMethodByIdQueryHandler(IGenericRepository<PaymentMethod> paymentMethodRepository, IMapper mapper)
    {
        _paymentMethodRepository = paymentMethodRepository;
        _mapper = mapper;
    }

    public async Task<PaymentMethodDetailsDto> Handle(GetPaymentMethodByIdQuery request, CancellationToken cancellationToken)
    {
        var paymentMethod = await _paymentMethodRepository.GetByIdAsync(request.Id);

        if (paymentMethod == null)
        {
            throw new NotFoundException(nameof(paymentMethod), request.Id);
        }

        return _mapper.Map<PaymentMethodDetailsDto>(paymentMethod);
    }
}
