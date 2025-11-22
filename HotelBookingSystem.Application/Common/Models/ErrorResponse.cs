
namespace HotelBookingSystem.Application.Common.Models;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string ErrorType { get; set; } = null!;
    public string Message { get; set; } = null!;
    public object? Details { get; set; }
}
