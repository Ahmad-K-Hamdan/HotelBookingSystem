using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Domain.Entities.Reviews;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Reviews.Queries.GetReviews;

public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, List<ReviewListDto>>
{
    private readonly IGenericRepository<Review> _reviewRepository;

    public GetReviewsQueryHandler(IGenericRepository<Review> reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<List<ReviewListDto>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        return await _reviewRepository.Query()
            .Include(r => r.Hotel)
            .Select(r => new ReviewListDto
            {
                Id = r.Id,
                HotelId = r.HotelId,
                HotelName = r.Hotel.HotelName,
                GuestId = r.GuestId,
                Rating = r.Rating,
                Comment = r.Comment,
                ReviewDate = r.ReviewDate
            })
            .ToListAsync(cancellationToken);
    }
}
