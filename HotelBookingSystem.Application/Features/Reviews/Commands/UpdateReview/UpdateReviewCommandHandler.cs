using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Reviews;
using MediatR;

namespace HotelBookingSystem.Application.Features.Reviews.Commands.UpdateReview;

public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, Unit>
{
    private readonly IGenericRepository<Review> _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReviewCommandHandler(IGenericRepository<Review> reviewRepository, IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.Id);

        if (review is null)
        {
            throw new NotFoundException(nameof(Review), request.Id);
        }

        review.Rating = request.Rating;
        review.Comment = request.Comment?.Trim();

        _reviewRepository.Update(review);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}
