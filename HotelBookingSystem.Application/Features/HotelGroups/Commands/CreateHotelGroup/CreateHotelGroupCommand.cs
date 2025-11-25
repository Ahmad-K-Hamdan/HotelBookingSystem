using MediatR;

namespace HotelBookingSystem.Application.Features.HotelGroups.Commands.CreateHotelGroup;

public record CreateHotelGroupCommand(string GroupName, string? Description) : IRequest<Guid>;
