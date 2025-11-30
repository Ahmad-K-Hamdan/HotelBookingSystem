using MediatR;

namespace HotelBookingSystem.Application.Features.HotelImages.Queries.GetHotelImages;

public record GetHotelImagesQuery() : IRequest<List<HotelImageListDto>>;
