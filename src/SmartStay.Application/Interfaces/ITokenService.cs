using System.Threading.Tasks;
using SmartStay.Domain.Entities;

namespace SmartStay.Application.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(User user);
    /// <summary>
    /// Creates a RefreshToken entity (for DB) and returns the raw token (for client).
    /// Never stores raw token in DB — only SHA-256 hash.
    /// </summary>
    (RefreshToken entity, string rawToken) CreateRefreshToken(User user, string? deviceInfo = null);
    string HashToken(string token);
}
