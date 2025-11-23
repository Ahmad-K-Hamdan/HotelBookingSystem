using HotelBookingSystem.Domain.Entities.Hotels;
using HotelBookingSystem.Domain.Entities.Payments;

namespace HotelBookingSystem.Domain.Entities.Discounts;

public class Discount
{
    public Guid Id { get; set; }
    public string DiscountDescription { get; set; } = null!;
    public decimal DiscountRate { get; set; }
    public bool IsActive { get; set; }

    public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
