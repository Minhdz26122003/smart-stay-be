using System;
using SmartStay.Domain.Enums;

namespace SmartStay.Domain.Entities;

public class Room : BaseEntity
{
    public Guid PropertyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Type { get; set; }
    public decimal BasePrice { get; set; }
    public double? AreaM2 { get; set; }
    public RoomStatus Status { get; set; } = RoomStatus.Available;
    public int? MaxOccupants { get; set; }
}
