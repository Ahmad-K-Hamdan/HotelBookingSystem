using HotelBookingSystem.Application.Common.Interfaces;

namespace HotelBookingSystem.Api.Middleware;

/// <summary>
/// Custom middleware for handling exceptions globally.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IEnumerable<IExceptionHandler> _handlers;

    /// <summary>
    /// Initializes a new instance of the ExceptionMiddleware class.
    /// </summary>
    /// <param name="next">The next request delegate in the pipeline.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="handlers">A list of all exception handlers.</param>
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IEnumerable<IExceptionHandler> handlers)
    {
        _next = next;
        _logger = logger;
        _handlers = handlers;
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
            var handler = _handlers.First(h => h.CanHandle(ex));
            var responseModel = handler.Handle(ex);

            context.Response.StatusCode = responseModel.StatusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(responseModel);
        }
    }
}
