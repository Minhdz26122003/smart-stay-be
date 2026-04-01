using System;

namespace SmartStay.Application.DTOs.VisitorLog;

public class VisitorLogDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string VisitorName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool StayOvernight { get; set; }
    public DateTime ArrivedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
