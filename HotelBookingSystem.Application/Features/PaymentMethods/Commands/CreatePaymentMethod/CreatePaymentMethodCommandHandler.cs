using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Payments;
using MediatR;

namespace HotelBookingSystem.Application.Features.PaymentMethods.Commands.CreatePaymentMethod;

public class CreatePaymentMethodCommandHandler : IRequestHandler<CreatePaymentMethodCommand, Guid>
{
    private readonly IGenericRepository<PaymentMethod> _paymentMethodRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePaymentMethodCommandHandler(
        IGenericRepository<PaymentMethod> paymentMethodRepository,
        IUnitOfWork unitOfWork)
    {
        _paymentMethodRepository = paymentMethodRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreatePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        var exists = await _paymentMethodRepository.FindAsync(c => c.MethodName.ToLower() == request.MethodName.ToLower());

        if (exists.Count > 0)
        {
            throw new DuplicateRecordException("Exact payment method already exists.");
        }

        var paymentMethod = new PaymentMethod
        {
            Id = Guid.NewGuid(),
            MethodName = request.MethodName
        };

        await _paymentMethodRepository.AddAsync(paymentMethod);
        await _unitOfWork.SaveChangesAsync();

        return paymentMethod.Id;
    }
}
