using System.Threading;
using System.Threading.Tasks;
using SmartStay.Domain.Interfaces;
using SmartStay.Infrastructure.Persistence;

namespace SmartStay.Infrastructure.Persistence;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);

    public void Dispose() => context.Dispose();
}
