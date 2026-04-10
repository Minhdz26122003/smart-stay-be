using System.Threading.Tasks;
using SmartStay.Domain.Entities;

namespace SmartStay.Domain.Interfaces;

public interface IOtpCodeRepository
{
    Task AddAsync(OtpCode code);
    Task<OtpCode?> FindLatestActiveAsync(string email, string purpose);
    Task<bool> FindVerifiedRecentAsync(string email, string purpose); // OTP đã verify trong 10 phút qua
    Task InvalidateAllAsync(string email, string purpose);
}
