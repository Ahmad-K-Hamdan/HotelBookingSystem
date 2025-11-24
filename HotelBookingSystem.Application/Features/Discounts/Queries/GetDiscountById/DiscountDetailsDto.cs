namespace HotelBookingSystem.Application.Features.Discounts.Queries.GetDiscountById;

public class DiscountDetailsDto
{
    public Guid Id { get; set; }
    public string DiscountDescription { get; set; } = null!;
    public decimal DiscountRate { get; set; }
    public bool IsActive { get; set; }
}
