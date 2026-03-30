using System;
using System.Threading.Tasks;
using SmartStay.Application.Interfaces;

namespace SmartStay.Infrastructure.BackgroundJobs;

public class DunningJob(INotificationService notificationService)
{
    public async Task ProcessUnpaidInvoicesAsync()
    {
        // TODO: Query DB for unpaid invoices older than X days
        // Send notifications
        // await notificationService.SendNotificationAsync(userId, "Payment Reminder", "Your invoice is overdue.");
        Console.WriteLine($"Running DunningJob at {DateTime.UtcNow}");
        await Task.CompletedTask;
    }
}
