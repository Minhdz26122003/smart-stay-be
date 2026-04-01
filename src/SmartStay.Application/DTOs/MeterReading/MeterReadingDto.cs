using System;

namespace SmartStay.Application.DTOs.MeterReading;

public class MeterReadingDto
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal OldUnit { get; set; }
    public decimal NewUnit { get; set; }
    public string? PhotoUrl { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public DateTime CreatedAt { get; set; }
}
