using System;
using System.Threading.Tasks;
using SmartStay.Domain.Entities;

namespace SmartStay.Domain.Interfaces;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> FindByTokenHashAsync(string tokenHash);
    Task RevokeAllForUserAsync(Guid userId);
}
