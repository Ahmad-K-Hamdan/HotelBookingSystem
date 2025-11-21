using FluentValidation;
using HotelBookingSystem.Application.Common.Exceptions;
using System.Net;

namespace HotelBookingSystem.Api.Middleware;

/// <summary>
/// Custom middleware for handling exceptions globally.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the ExceptionMiddleware class.
    /// </summary>
    /// <param name="next">The next request delegate in the pipeline.</param>
    /// <param name="logger">The logger instance.</param>
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware to handle HTTP requests asynchronously.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, _logger);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger logger)
    {
        context.Response.ContentType = "application/json";

        int statusCode = (int)HttpStatusCode.InternalServerError;
        object responseBody = new
        {
            Message = "An unexpected error occurred.",
            ErrorType = "InternalServerError"
        };

        logger.LogError(ex, "An unhandled exception occurred during request processing.");

        switch (ex)
        {
            case ValidationException validationEx:
                statusCode = (int)HttpStatusCode.BadRequest;
                responseBody = new
                {
                    Message = "One or more validation errors occurred.",
                    ErrorType = validationEx.GetType().Name,
                    Errors = validationEx.Errors
                        .GroupBy(f => f.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        )
                };
                break;

            case IdentityException identityEx:
                statusCode = (int)HttpStatusCode.BadRequest;
                responseBody = new
                {
                    Message = identityEx.Message,
                    ErrorType = identityEx.GetType().Name
                };
                break;
        }

        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsJsonAsync(responseBody);
    }
}
