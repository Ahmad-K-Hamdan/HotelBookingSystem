using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Payments;
using MediatR;

namespace HotelBookingSystem.Application.Features.PaymentMethods.Commands.DeletePaymentMethod;

public class DeletePaymentMethodCommandHandler : IRequestHandler<DeletePaymentMethodCommand, Unit>
{
    private readonly IGenericRepository<PaymentMethod> _paymentMethodRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePaymentMethodCommandHandler(IGenericRepository<PaymentMethod> paymentMethodRepository, IUnitOfWork unitOfWork)
    {
        _paymentMethodRepository = paymentMethodRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeletePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        var paymentMethod = await _paymentMethodRepository.GetByIdAsync(request.Id);

        if (paymentMethod == null)
        {
            throw new NotFoundException(nameof(paymentMethod), request.Id);
        }

        _paymentMethodRepository.Delete(paymentMethod);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}