using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Common.Models;
using Microsoft.Extensions.Logging;
using System.Net;

namespace HotelBookingSystem.Application.Common.Exceptions.Handlers;

public class IdentityExceptionHandler : IExceptionHandler
{
    private readonly ILogger<IdentityExceptionHandler> _logger;

    public IdentityExceptionHandler(ILogger<IdentityExceptionHandler> logger)
    {
        _logger = logger;
    }

    public bool CanHandle(Exception ex) => ex is IdentityException;

    public ErrorResponse Handle(Exception ex)
    {
        _logger.LogError(ex, "Identity exception occurred");

        return new ErrorResponse
        {
            StatusCode = (int)HttpStatusCode.BadRequest,
            ErrorType = nameof(IdentityException),
            Message = ex.Message
        };
    }
}
