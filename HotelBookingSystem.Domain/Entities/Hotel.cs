namespace HotelBookingSystem.Domain.Entities;

public class Hotel
{
    public Guid Id { get; set; }
    public Guid HotelGroupId { get; set; }
    public Guid CityId { get; set; }
    public Guid? DiscountId { get; set; }
    public string HotelName { get; set; } = null!;
    public string HotelAddress { get; set; } = null!;
    public int StarRating { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public City City { get; set; } = null!;
    public HotelGroup HotelGroup { get; set; } = null!;
    public Discount? Discount { get; set; }
    public ICollection<HotelRoomType> RoomTypes { get; set; } = new List<HotelRoomType>();
    public ICollection<HotelImage> Images { get; set; } = new List<HotelImage>();
    public ICollection<HotelAmenity> HotelAmenities { get; set; } = new List<HotelAmenity>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
