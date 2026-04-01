using System;
using SmartStay.Domain.Enums;

namespace SmartStay.Application.DTOs.Vehicle;

public class CreateVehicleRequest
{
    public string PlateNumber { get; set; } = string.Empty;
    public VehicleType VehicleType { get; set; }
    public string? PhotoUrl { get; set; }
}
