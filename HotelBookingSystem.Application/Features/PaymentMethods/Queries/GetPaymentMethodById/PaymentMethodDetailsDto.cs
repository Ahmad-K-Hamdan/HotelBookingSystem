namespace HotelBookingSystem.Application.Features.PaymentMethods.Queries.GetPaymentMethodById;

public class PaymentMethodDetailsDto
{
    public Guid Id { get; set; }
    public string MethodName { get; set; } = null!;
}
