using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Interfaces;
using SmartStay.Infrastructure.Persistence;

namespace SmartStay.Infrastructure.Repositories;

public class OtpCodeRepository(AppDbContext db) : IOtpCodeRepository
{
    public async Task AddAsync(OtpCode code) => await db.OtpCodes.AddAsync(code);

    public async Task<OtpCode?> FindLatestActiveAsync(string email, string purpose)
        => await db.OtpCodes
            .IgnoreQueryFilters()
            .Where(x => x.Email == email
                        && x.Purpose == purpose
                        && !x.IsUsed
                        && !x.IsDeleted
                        && x.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();

    public async Task<bool> FindVerifiedRecentAsync(string email, string purpose)
        => await db.OtpCodes
            .IgnoreQueryFilters()
            .AnyAsync(x => x.Email == email
                           && x.Purpose == purpose
                           && x.IsUsed
                           && !x.IsDeleted
                           && x.UpdatedAt >= DateTime.UtcNow.AddMinutes(-10));

    public async Task InvalidateAllAsync(string email, string purpose)
    {
        var codes = await db.OtpCodes
            .IgnoreQueryFilters()
            .Where(x => x.Email == email && x.Purpose == purpose && !x.IsUsed)
            .ToListAsync();

        foreach (var c in codes)
        {
            c.IsUsed = true;
            c.UpdatedAt = DateTime.UtcNow;
        }
    }
}
