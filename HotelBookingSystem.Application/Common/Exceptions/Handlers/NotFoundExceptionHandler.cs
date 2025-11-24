using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Common.Models;
using Microsoft.Extensions.Logging;
using System.Net;

namespace HotelBookingSystem.Application.Common.Exceptions.Handlers;

public class NotFoundExceptionHandler : IExceptionHandler
{
    private readonly ILogger<NotFoundExceptionHandler> _logger;

    public NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger)
    {
        _logger = logger;
    }

    public bool CanHandle(Exception ex) => ex is NotFoundException;

    public ErrorResponse Handle(Exception ex)
    {
        _logger.LogError(ex, "Not found exception occurred");

        return new ErrorResponse
        {
            StatusCode = (int)HttpStatusCode.NotFound,
            ErrorType = nameof(NotFoundException),
            Message = ex.Message
        };
    }
}
