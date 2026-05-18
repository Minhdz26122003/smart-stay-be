using System;
using SmartStay.Domain.Enums;

namespace SmartStay.Domain.Entities;

public class Invoice : BaseEntity
{
    public Guid RoomId { get; set; }
    public Guid ContractId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public string BreakdownJson { get; set; } = string.Empty; // Store as JSONB in PostgreSQL
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;

    public Room? Room { get; set; }
    public Contract? Contract { get; set; }
}
