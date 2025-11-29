using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Guests;
using HotelBookingSystem.Domain.Entities.Hotels;
using HotelBookingSystem.Domain.Entities.Reviews;
using MediatR;

namespace HotelBookingSystem.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Guid>
{
    private readonly IGenericRepository<Review> _reviewRepository;
    private readonly IGenericRepository<Hotel> _hotelRepository;
    private readonly IGenericRepository<Guest> _guestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReviewCommandHandler(
        IGenericRepository<Review> reviewRepository,
        IGenericRepository<Hotel> hotelRepository,
        IGenericRepository<Guest> guestRepository,
        IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _hotelRepository = hotelRepository;
        _guestRepository = guestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        _ = await _hotelRepository.GetByIdAsync(request.HotelId)
            ?? throw new NotFoundException("Hotel", request.HotelId);

        _ = await _guestRepository.GetByIdAsync(request.GuestId)
            ?? throw new NotFoundException("Guest", request.GuestId);

        var review = new Review
        {
            Id = Guid.NewGuid(),
            HotelId = request.HotelId,
            GuestId = request.GuestId,
            Rating = request.Rating,
            Comment = request.Comment?.Trim(),
            ReviewDate = DateTime.UtcNow
        };

        await _reviewRepository.AddAsync(review);
        await _unitOfWork.SaveChangesAsync();

        return review.Id;
    }
}
