using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Payments;
using MediatR;

namespace HotelBookingSystem.Application.Features.PaymentMethods.Commands.UpdatePaymentMethod;

public class UpdatePaymentMethodCommandHandler : IRequestHandler<UpdatePaymentMethodCommand, Unit>
{
    private readonly IGenericRepository<PaymentMethod> _paymentMethodRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePaymentMethodCommandHandler(IGenericRepository<PaymentMethod> paymentMethodRepository, IUnitOfWork unitOfWork)
    {
        _paymentMethodRepository = paymentMethodRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdatePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        var paymentMethod = await _paymentMethodRepository.GetByIdAsync(request.Id);

        if (paymentMethod == null)
        {
            throw new NotFoundException(nameof(paymentMethod), request.Id);
        }

        paymentMethod.MethodName = request.MethodName;

        _paymentMethodRepository.Update(paymentMethod);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}