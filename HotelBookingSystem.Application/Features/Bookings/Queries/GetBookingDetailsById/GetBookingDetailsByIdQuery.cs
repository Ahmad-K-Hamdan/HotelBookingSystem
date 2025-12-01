using HotelBookingSystem.Application.Features.Bookings.Queries.GetBookingDetailsById.Dtos;
using MediatR;

namespace HotelBookingSystem.Application.Features.Bookings.Queries.GetBookingDetailsById;

public record GetBookingDetailsByIdQuery(Guid Id) : IRequest<BookingDetailsDto>;
