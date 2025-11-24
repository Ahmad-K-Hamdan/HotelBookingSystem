using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Discounts;
using MediatR;

namespace HotelBookingSystem.Application.Features.Discounts.Commands.CreateDiscount;

public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, Guid>
{
    private readonly IGenericRepository<Discount> _discountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDiscountCommandHandler(
        IGenericRepository<Discount> discountRepository,
        IUnitOfWork unitOfWork)
    {
        _discountRepository = discountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
    {
        var exists = await _discountRepository.FindAsync(c => c.DiscountDescription.ToLower() == request.DiscountDescription.ToLower()
                                                                && c.DiscountRate == request.DiscountRate);

        if (exists.Count > 0)
        {
            throw new DuplicateRecordException("Exact discount already exists.");
        }

        var discount = new Discount
        {
            Id = Guid.NewGuid(),
            DiscountDescription = request.DiscountDescription,
            DiscountRate = request.DiscountRate,
            IsActive = request.IsActive,
        };

        await _discountRepository.AddAsync(discount);
        await _unitOfWork.SaveChangesAsync();

        return discount.Id;
    }
}
