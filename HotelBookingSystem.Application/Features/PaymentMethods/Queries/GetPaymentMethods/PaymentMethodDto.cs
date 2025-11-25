namespace HotelBookingSystem.Application.Features.PaymentMethods.Queries.GetPaymentMethods;

public class PaymentMethodDto
{
    public Guid Id { get; set; }
    public string MethodName { get; set; } = null!;
}
