using FluentValidation;
using HotelBookingSystem.Application.Common.Interfaces;
using HotelBookingSystem.Application.Common.Models;
using Microsoft.Extensions.Logging;
using System.Net;

namespace HotelBookingSystem.Application.Common.Exceptions.Handlers;

public class ValidationExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ValidationExceptionHandler> _logger;

    public ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger)
    {
        _logger = logger;
    }

    public bool CanHandle(Exception ex) => ex is ValidationException;

    public ErrorResponse Handle(Exception ex)
    {
        _logger.LogError(ex, "Validation exception occurred");

        var valEx = (ValidationException)ex;

        var errors = valEx.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToList()
            );

        return new ErrorResponse
        {
            StatusCode = (int)HttpStatusCode.BadRequest,
            ErrorType = nameof(ValidationException),
            Message = "One or more validation errors occurred.",
            Details = errors
        };
    }
}
