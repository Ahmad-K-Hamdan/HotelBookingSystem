using MediatR;

namespace HotelBookingSystem.Application.Features.HotelGroups.Queries.GetHotelGroupById;

public record GetHotelGroupByIdQuery(Guid Id) : IRequest<HotelGroupDetailsDto>;
