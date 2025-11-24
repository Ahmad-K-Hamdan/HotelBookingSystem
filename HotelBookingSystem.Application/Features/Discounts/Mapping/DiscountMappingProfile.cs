using AutoMapper;
using HotelBookingSystem.Application.Features.Discounts.Queries.GetDiscountById;
using HotelBookingSystem.Domain.Entities.Discounts;

namespace HotelBookingSystem.Application.Features.Discounts.Mapping;

public class DiscountMappingProfile : Profile
{
    public DiscountMappingProfile()
    {
        CreateMap<Discount, DiscountDetailsDto>();
    }
}
