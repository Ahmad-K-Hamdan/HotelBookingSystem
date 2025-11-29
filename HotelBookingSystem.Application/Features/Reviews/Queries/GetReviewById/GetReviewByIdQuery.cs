using HotelBookingSystem.Application.Features.Reviews.Queries.GetReviewById.Dtos;
using MediatR;

namespace HotelBookingSystem.Application.Features.Reviews.Queries.GetReviewById;

public record GetReviewByIdQuery(Guid Id) : IRequest<ReviewDetailsDto>;
