using System;
using SmartStay.Domain.Enums;

namespace SmartStay.Domain.Entities;

public class Vehicle : BaseEntity
{
    public Guid TenantId { get; set; }
    public string PlateNumber { get; set; } = string.Empty;
    public VehicleType VehicleType { get; set; }
    public string? PhotoUrl { get; set; }
}
