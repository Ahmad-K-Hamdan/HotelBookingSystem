using HotelBookingSystem.Application.Common.Exceptions;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Features.Reviews.Queries.GetReviewById.Dtos;
using HotelBookingSystem.Domain.Entities.Reviews;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Application.Features.Reviews.Queries.GetReviewById;

public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, ReviewDetailsDto>
{
    private readonly IGenericRepository<Review> _reviewRepository;

    public GetReviewByIdQueryHandler(IGenericRepository<Review> reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<ReviewDetailsDto> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.Query()
            .Include(r => r.Hotel)
                .ThenInclude(h => h.City)
            .Include(r => r.Guest)
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (review is null)
        {
            throw new NotFoundException(nameof(Review), request.Id);
        }

        return new ReviewDetailsDto
        {
            Id = review.Id,
            Rating = review.Rating,
            Comment = review.Comment,
            ReviewDate = review.ReviewDate,
            Hotel = new HotelSummaryDto
            {
                Id = review.Hotel.Id,
                HotelName = review.Hotel.HotelName,
                CityName = review.Hotel.City.CityName,
                CountryName = review.Hotel.City.CountryName,
                StarRating = review.Hotel.StarRating
            },
            Guest = new GuestSummaryDto
            {
                Id = review.Guest.Id,
                UserId = review.Guest.UserId,
                HomeCountry = review.Guest.HomeCountry
            }
        };
    }
}
