using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SmartStay.Application.Interfaces;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Interfaces;
// Assuming you have a way to reference the Hub contexts from Infrastructure, or better, keep the Hub interface in Application layer.
// Actually, it's better to abstract the Hub communication. We will use a wrapper or directly inject IHubContext if referenced.
// To avoid referencing API project from Infrastructure, we often define an INotificationHub interface in Application.
// Let's assume we use an abstract interface INotificationClient.

namespace SmartStay.Infrastructure.Services;

// This service saves the notification to DB and attempts to push it.
public class NotificationService(IUnitOfWork unitOfWork, IRepository<Notification> notificationRepository) : INotificationService
{
    public async Task SendNotificationAsync(Guid userId, string title, string message)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Body = message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await notificationRepository.AddAsync(notification);
        await unitOfWork.CommitAsync();

        // Push notification logic using SignalR/Firebase is typically delegated to a specific provider
        // e.g. _hubContext.Clients.Group(userId.ToString()).SendAsync("ReceiveNotification", notification);
        // We will configure this when wiring up the API. For now, we save it.
    }
}
