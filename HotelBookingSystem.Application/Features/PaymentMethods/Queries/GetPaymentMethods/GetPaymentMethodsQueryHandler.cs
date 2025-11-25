using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Payments;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.PaymentMethods.Queries.GetPaymentMethods;

public class GetPaymentMethodsQueryHandler : IRequestHandler<GetPaymentMethodsQuery, List<PaymentMethodDto>>
{
    private readonly IGenericRepository<PaymentMethod> _paymentMethodRepository;

    public GetPaymentMethodsQueryHandler(IGenericRepository<PaymentMethod> paymentMethodRepository)
    {
        _paymentMethodRepository = paymentMethodRepository;
    }

    public async Task<List<PaymentMethodDto>> Handle(GetPaymentMethodsQuery request, CancellationToken cancellationToken)
    {
        return await _paymentMethodRepository
            .Query()
            .OrderBy(c => c)
            .Select(c => new PaymentMethodDto
            {
                Id = c.Id,
                MethodName = c.MethodName
            })
            .ToListAsync(cancellationToken);
    }
}
