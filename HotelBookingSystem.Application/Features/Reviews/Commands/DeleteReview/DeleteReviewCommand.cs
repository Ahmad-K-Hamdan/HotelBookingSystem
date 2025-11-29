using MediatR;

namespace HotelBookingSystem.Application.Features.Reviews.Commands.DeleteReview;

public record DeleteReviewCommand(Guid Id) : IRequest<Unit>;
