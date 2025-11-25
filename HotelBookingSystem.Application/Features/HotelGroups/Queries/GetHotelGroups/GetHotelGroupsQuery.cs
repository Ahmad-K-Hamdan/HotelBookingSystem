using MediatR;

namespace HotelBookingSystem.Application.Features.HotelGroups.Queries.GetHotelGroups;

public record GetHotelGroupsQuery() : IRequest<List<HotelGroupDto>>;
