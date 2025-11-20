using HotelBookingSystem.Application.Common.Exceptions;
using System.Net;

namespace HotelBookingSystem.Api.Middleware;

/// <summary>
/// Custom middleware for handling exceptions globally.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the ExceptionMiddleware class.
    /// </summary>
    /// <param name="next"> The next request delegate in the pipeline.</param>
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        int statusCode = ex switch
        {
            IdentityException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var response = new
        {
            Message = ex.Message!,
            ErrorType = ex.GetType().Name
        };

        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsJsonAsync(response);
    }
}
