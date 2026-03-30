using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Interfaces;
using SmartStay.Infrastructure.Persistence;

namespace SmartStay.Infrastructure.Repositories;

public class UserRepository(AppDbContext context)
    : GenericRepository<User>(context), IUserRepository
{
    public async Task<User?> FindByPhoneAsync(string phone)
        => await _dbSet.FirstOrDefaultAsync(u => u.Phone == phone);

    public async Task<User?> FindByEmailAsync(string email)
        => await _dbSet.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<bool> ExistsByPhoneAsync(string phone)
        => await _dbSet.AnyAsync(u => u.Phone == phone);

    public async Task<bool> ExistsByEmailAsync(string email)
        => await _dbSet.AnyAsync(u => u.Email == email);
}
