using System;
using System.Threading.Tasks;
using SmartStay.Domain.Entities;

namespace SmartStay.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> FindByPhoneAsync(string phone);
    Task<User?> FindByEmailAsync(string email);
    Task<bool> ExistsByPhoneAsync(string phone);
    Task<bool> ExistsByEmailAsync(string email);
}
