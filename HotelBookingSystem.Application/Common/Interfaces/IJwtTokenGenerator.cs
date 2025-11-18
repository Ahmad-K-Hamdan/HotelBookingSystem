using HotelBookingSystem.Application.Common.Models;
using System.Security.Claims;

namespace HotelBookingSystem.Application.Common.Interfaces;
public interface IJwtTokenGenerator
{
    string GenerateToken(AuthenticatedUser user, IEnumerable<Claim> userClaims, IEnumerable<string> roles);
}
