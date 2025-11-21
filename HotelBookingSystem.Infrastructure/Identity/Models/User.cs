using Microsoft.AspNetCore.Identity;

namespace HotelBookingSystem.Infrastructure.Identity.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
}
