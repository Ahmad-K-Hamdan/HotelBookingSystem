using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Common.Models;
using Microsoft.Extensions.Logging;
using System.Net;
namespace HotelBookingSystem.Application.Common.Exceptions.Handlers;

public class UnauthorizedAccessExceptionHandler : IExceptionHandler
{
    private readonly ILogger<UnauthorizedAccessExceptionHandler> _logger;

    public UnauthorizedAccessExceptionHandler(ILogger<UnauthorizedAccessExceptionHandler> logger)
    {
        _logger = logger;
    }

    public bool CanHandle(Exception ex) => ex is UnauthorizedAccessException;

    public ErrorResponse Handle(Exception ex)
    {
        _logger.LogError(ex, "Unauthorized access exception occurred");

        return new ErrorResponse
        {
            StatusCode = (int)HttpStatusCode.Unauthorized,
            ErrorType = nameof(UnauthorizedAccessException),
            Message = ex.Message
        };
    }
}
