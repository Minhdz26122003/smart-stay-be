using System;
using SmartStay.Domain.Enums;

namespace SmartStay.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; }
    public string? Payload { get; set; } // JSON string

    public User? User { get; set; }
}
