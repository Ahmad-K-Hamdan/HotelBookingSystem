using MediatR;

namespace HotelBookingSystem.Application.Features.Reviews.Queries.GetReviews;

public record GetReviewsQuery() : IRequest<List<ReviewListDto>>;
