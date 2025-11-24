using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Discounts;
using MediatR;

namespace HotelBookingSystem.Application.Features.Discounts.Commands.UpdateDiscount;

public class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand, Unit>
{
    private readonly IGenericRepository<Discount> _discountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDiscountCommandHandler(IGenericRepository<Discount> discountRepository, IUnitOfWork unitOfWork)
    {
        _discountRepository = discountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = await _discountRepository.GetByIdAsync(request.Id);

        if (discount == null)
        {
            throw new NotFoundException(nameof(discount), request.Id);
        }

        discount.DiscountDescription = request.DiscountDescription;
        discount.DiscountRate = request.DiscountRate;
        discount.IsActive = request.IsActive;

        _discountRepository.Update(discount);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}