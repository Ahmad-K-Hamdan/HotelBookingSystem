using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Common.Models;
using Microsoft.Extensions.Logging;
using System.Net;

namespace HotelBookingSystem.Application.Common.Exceptions.Handlers;

public class DuplicateRecordExceptionHandler : IExceptionHandler
{
    private readonly ILogger<DuplicateRecordExceptionHandler> _logger;

    public DuplicateRecordExceptionHandler(ILogger<DuplicateRecordExceptionHandler> logger)
    {
        _logger = logger;
    }

    public bool CanHandle(Exception ex) => ex is DuplicateRecordException;

    public ErrorResponse Handle(Exception ex)
    {
        _logger.LogError(ex, "Duplicate record exception occurred");

        return new ErrorResponse
        {
            StatusCode = (int)HttpStatusCode.BadRequest,
            ErrorType = nameof(DuplicateRecordException),
            Message = ex.Message
        };
    }
}
