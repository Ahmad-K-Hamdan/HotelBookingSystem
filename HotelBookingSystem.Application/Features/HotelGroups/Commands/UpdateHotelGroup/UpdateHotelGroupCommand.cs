using MediatR;

namespace HotelBookingSystem.Application.Features.HotelGroups.Commands.UpdateHotelGroup;

public record UpdateHotelGroupCommand(Guid Id, string GroupName, string? Description) : IRequest<Unit>;

