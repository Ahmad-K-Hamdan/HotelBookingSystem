using System.Security.Claims;
using HotelBookingSystem.Application.Common.Interfaces;

namespace HotelBookingSystem.Api.Services;

/// <summary>
/// Provides information about the currently authenticated user 
/// by reading data from the active HttpContext.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the CurrentUserService class.
    /// </summary>
    /// <param name="httpContextAccessor">Accessor used to read the current HTTP context.</param>
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Gets the ID of the currently authenticated user, extracted from the JWT token.
    /// </summary>
    public string? UserId =>
        _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value
            ?? _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}
