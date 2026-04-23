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

    public TenantDto? Tenant { get; set; }
    public SmartStay.Application.DTOs.Contract.ContractDto? Contract { get; set; }
    public System.Collections.Generic.List<SmartStay.Application.DTOs.Invoice.InvoiceDto>? Invoices { get; set; }
    public System.Collections.Generic.List<SmartStay.Application.DTOs.InventoryItem.InventoryItemDto>? InventoryItems { get; set; }
    public System.Collections.Generic.List<SmartStay.Application.DTOs.MeterReading.MeterReadingDto>? LatestMeterReadings { get; set; }
}
