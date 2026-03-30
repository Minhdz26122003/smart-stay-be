using System;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartStay.Application.Interfaces;
using SmartStay.Domain.Entities;

namespace SmartStay.Infrastructure.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    private readonly string _secret = configuration["Jwt:Secret"]
        ?? throw new InvalidOperationException("Jwt:Secret is not configured.");
    private readonly string _issuer = configuration["Jwt:Issuer"] ?? "SmartStayAPI";
    private readonly string _audience = configuration["Jwt:Audience"] ?? "SmartStayApp";
    private readonly int _accessTokenExpiryMinutes =
        int.TryParse(configuration["Jwt:AccessTokenExpiryMinutes"], out var m) ? m : 15;
    private readonly int _refreshTokenExpiryDays =
        int.TryParse(configuration["Jwt:RefreshTokenExpiryDays"], out var d) ? d : 14;

    public string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Name, user.FullName),
            new("phone", user.Phone),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_accessTokenExpiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Creates a RefreshToken entity with hashed token for DB storage.
    /// Returns the (entity, rawToken) tuple — caller sends rawToken to client, saves entity to DB.
    /// </summary>
    public (RefreshToken entity, string rawToken) CreateRefreshToken(User user, string? deviceInfo = null)
    {
        var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        var entity = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = HashToken(rawToken), // Only hash stored in DB
            ExpiresAt = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays),
            DeviceInfo = deviceInfo
        };

        return (entity, rawToken);
    }

    public string HashToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(bytes);
    }
}
