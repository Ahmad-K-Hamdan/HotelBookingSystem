using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace HotelBookingSystem.Application.Common.Exceptions.Handlers;

public class DefaultExceptionHandler : IExceptionHandler
{
    private readonly ILogger<DefaultExceptionHandler> _logger;

    public DefaultExceptionHandler(ILogger<DefaultExceptionHandler> logger)
    {
        _logger = logger;
    }

    public bool CanHandle(Exception ex) => true;

    public ErrorResponse Handle(Exception ex)
    {
        _logger.LogError(ex, "Unhandled exception occurred");

        return new ErrorResponse
        {
            StatusCode = StatusCodes.Status500InternalServerError,
            Message = "An unexpected error occurred.",
            ErrorType = "InternalServerError"
        };
    }
}
