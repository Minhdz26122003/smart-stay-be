using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Interfaces;
using SmartStay.Infrastructure.Persistence;

namespace SmartStay.Infrastructure.Repositories;

public class RefreshTokenRepository(AppDbContext context)
    : GenericRepository<RefreshToken>(context), IRefreshTokenRepository
{
    public async Task<RefreshToken?> FindByTokenHashAsync(string tokenHash)
        => await _dbSet.FirstOrDefaultAsync(r => r.TokenHash == tokenHash);

    public async Task RevokeAllForUserAsync(Guid userId)
    {
        var tokens = await _dbSet
            .Where(r => r.UserId == userId && !r.IsRevoked)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
            token.UpdatedAt = DateTime.UtcNow;
        }
    }
}
