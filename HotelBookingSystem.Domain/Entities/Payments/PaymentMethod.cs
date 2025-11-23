namespace HotelBookingSystem.Domain.Entities.Payments;

public class PaymentMethod
{
    public Guid Id { get; set; }
    public string MethodName { get; set; } = null!;

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
