namespace HotelBookingSystem.Application.Common.Exceptions;

public class DuplicateRecordException : Exception
{
    public DuplicateRecordException(string message) : base(message)
    {
    }
}