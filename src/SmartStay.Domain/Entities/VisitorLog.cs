using System;

namespace SmartStay.Domain.Entities;

public class VisitorLog : BaseEntity
{
    public Guid TenantId { get; set; }
    public string VisitorName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool StayOvernight { get; set; }
    public DateTime ArrivedAt { get; set; } = DateTime.UtcNow;

    public User? Tenant { get; set; }
}
