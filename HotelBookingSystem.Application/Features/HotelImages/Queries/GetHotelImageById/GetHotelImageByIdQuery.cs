using MediatR;

namespace HotelBookingSystem.Application.Features.HotelImages.Queries.GetHotelImageById;

public record GetHotelImageByIdQuery(Guid Id) : IRequest<HotelImageDetailsDto>;
