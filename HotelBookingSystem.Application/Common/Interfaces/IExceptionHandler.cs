using HotelBookingSystem.Application.Common.Models;

namespace HotelBookingSystem.Application.Common.Interfaces;

public interface IExceptionHandler
{
    bool CanHandle(Exception ex);
    ErrorResponse Handle(Exception ex);
}
