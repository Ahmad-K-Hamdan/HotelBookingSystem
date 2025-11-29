using MediatR;

namespace HotelBookingSystem.Application.Features.HotelRoomTypes.Commands.CreateHotelRoomType;

public record CreateHotelRoomTypeCommand(CreateHotelRoomTypeDto HotelRoomType) : IRequest<Guid>;
