using HotelBookingSystem.Application.Common.Exceptions;
using System.Net;

namespace HotelBookingSystem.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

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
