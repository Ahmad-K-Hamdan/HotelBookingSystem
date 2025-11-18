namespace HotelBookingSystem.Application.Common.Exceptions;
public class IdentityException : Exception
{
    public IdentityException(string message) : base(message)
    {
    }
}