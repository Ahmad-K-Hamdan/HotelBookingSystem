using AutoMapper;
using HotelBookingSystem.Application.Features.PaymentMethods.Queries.GetPaymentMethodById;
using HotelBookingSystem.Domain.Entities.Payments;

namespace HotelBookingSystem.Application.Features.PaymentMethods.Mapping;

public class PaymentMethodMappingProfile : Profile
{
    public PaymentMethodMappingProfile()
    {
        CreateMap<PaymentMethod, PaymentMethodDetailsDto>();
    }
}
