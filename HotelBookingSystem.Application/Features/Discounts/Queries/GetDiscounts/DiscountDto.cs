namespace HotelBookingSystem.Application.Features.Discounts.Queries.GetDiscounts;

public class DiscountDto
{
    public Guid Id { get; set; }
    public string DiscountDescription { get; set; } = null!;
    public decimal DiscountRate { get; set; }
    public bool IsActive { get; set; }
}
