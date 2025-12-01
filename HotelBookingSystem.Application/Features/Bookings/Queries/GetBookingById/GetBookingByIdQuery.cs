using HotelBookingSystem.Application.Features.Bookings.Queries.GetBookingById.Dtos;
using MediatR;

namespace HotelBookingSystem.Application.Features.Bookings.Queries.GetBookingById;

public record GetBookingByIdQuery(Guid Id) : IRequest<BookingDetailsDto>;
