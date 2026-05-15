using System;
using System.Collections.Generic;
using SmartStay.Domain.Enums;

namespace SmartStay.Application.DTOs.Ticket;

public class TicketDto
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public string? RoomName { get; set; }  
    public string? TenantName { get; set; } 
    public Guid TenantId { get; set; }
    public TicketCategory Category { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> PhotoUrls { get; set; } = new();
    public TicketStatus Status { get; set; }
    public TicketPriority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
}
