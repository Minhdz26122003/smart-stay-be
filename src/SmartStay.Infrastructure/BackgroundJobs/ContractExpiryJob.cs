using System;
using System.Threading.Tasks;
using SmartStay.Application.Interfaces;

namespace SmartStay.Infrastructure.BackgroundJobs;

public class ContractExpiryJob(INotificationService notificationService)
{
    public async Task CheckExpiringContractsAsync()
    {
        // TODO: Query DB for contracts expiring in <= 30 days
        // Send notifications
        Console.WriteLine($"Running ContractExpiryJob at {DateTime.UtcNow}");
        await Task.CompletedTask;
    }
}
