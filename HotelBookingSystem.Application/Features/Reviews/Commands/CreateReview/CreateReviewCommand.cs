using MediatR;

namespace HotelBookingSystem.Application.Features.Reviews.Commands.CreateReview;

public record CreateReviewCommand(Guid HotelId, Guid GuestId, int Rating, string? Comment) : IRequest<Guid>;
