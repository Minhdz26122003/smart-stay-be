using System;
using System.Collections.Generic;
using SmartStay.Domain.Enums;

namespace SmartStay.Domain.Entities;

public class Ticket : BaseEntity
{
    public Guid RoomId { get; set; }
    public Guid TenantId { get; set; }
    public TicketCategory Category { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> PhotoUrls { get; set; } = new();
    public TicketStatus Status { get; set; } = TicketStatus.Pending;
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;

    public Room? Room { get; set; }
    public User? Tenant { get; set; }
}
