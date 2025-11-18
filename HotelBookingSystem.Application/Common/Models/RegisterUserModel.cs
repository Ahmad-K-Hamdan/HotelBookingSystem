namespace HotelBookingSystem.Application.Common.Models;

public class RegisterUserModel
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime? BirthDate { get; set; }
}
