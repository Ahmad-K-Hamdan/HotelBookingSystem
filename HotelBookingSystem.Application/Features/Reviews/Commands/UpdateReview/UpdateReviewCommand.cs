using MediatR;

namespace HotelBookingSystem.Application.Features.Reviews.Commands.UpdateReview;

public record UpdateReviewCommand(Guid Id, int Rating, string? Comment) : IRequest<Unit>;
