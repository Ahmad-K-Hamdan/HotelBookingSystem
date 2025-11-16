namespace HotelBookingSystem.Application.Common.Models;
public class AuthResult
{
    public bool Succeeded { get; set; }
    public string Token { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string Error { get; set; } = null!;
}
