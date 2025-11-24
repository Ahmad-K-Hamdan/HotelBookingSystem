using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Discounts;
using MediatR;

namespace HotelBookingSystem.Application.Features.Discounts.Commands.DeleteDiscount;

public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand, Unit>
{
    private readonly IGenericRepository<Discount> _discountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDiscountCommandHandler(IGenericRepository<Discount> discountRepository, IUnitOfWork unitOfWork)
    {
        _discountRepository = discountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = await _discountRepository.GetByIdAsync(request.Id);

        if (discount == null)
        {
            throw new NotFoundException(nameof(discount), request.Id);
        }

        _discountRepository.Delete(discount);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}