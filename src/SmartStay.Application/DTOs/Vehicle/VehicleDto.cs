using System;
using SmartStay.Domain.Enums;

namespace SmartStay.Application.DTOs.Vehicle;

public class VehicleDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string PlateNumber { get; set; } = string.Empty;
    public VehicleType VehicleType { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
