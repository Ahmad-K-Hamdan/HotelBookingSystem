using MediatR;

namespace HotelBookingSystem.Application.Features.HotelGroups.Commands.DeleteHotelGroup;

public record DeleteHotelGroupCommand(Guid Id) : IRequest<Unit>;
