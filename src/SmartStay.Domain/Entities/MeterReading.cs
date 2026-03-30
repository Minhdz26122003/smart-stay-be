using System;

namespace SmartStay.Domain.Entities;

public class MeterReading : BaseEntity
{
    public Guid RoomId { get; set; }
    public string Type { get; set; } = string.Empty; // Electric or Water
    public decimal OldUnit { get; set; }
    public decimal NewUnit { get; set; }
    public string? PhotoUrl { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}
