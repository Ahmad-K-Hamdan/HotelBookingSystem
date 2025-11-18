using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotelBookingSystem.Application.Common.Models;
using HotelBookingSystem.Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HotelBookingSystem.Infrastructure.Identity.JwtTokens;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateToken(AuthenticatedUser user, IEnumerable<Claim> claims, IEnumerable<string> roles)
    {
        var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));

        var allClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("firstName", user.FirstName),
            new("lastName", user.LastName)
        };

        allClaims.AddRange(claims);
        allClaims.AddRange(roleClaims);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: allClaims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
