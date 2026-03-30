using System;
using SmartStay.Domain.Enums;

namespace SmartStay.Application.DTOs.Room;

public class RoomDto
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Type { get; set; }
    public decimal BasePrice { get; set; }
    public double? AreaM2 { get; set; }
    public RoomStatus Status { get; set; }
    public int? MaxOccupants { get; set; }
}
