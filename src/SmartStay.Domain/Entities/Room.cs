using System;
using System.Collections.Generic;
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

    public Property? Property { get; set; }
    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    public ICollection<Listing> Listings { get; set; } = new List<Listing>();
    public ICollection<MeterReading> MeterReadings { get; set; } = new List<MeterReading>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public ICollection<ServiceConfig> ServiceConfigs { get; set; } = new List<ServiceConfig>();
    public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
}
