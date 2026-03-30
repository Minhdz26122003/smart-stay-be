using System;
using SmartStay.Domain.Enums;

namespace SmartStay.Application.DTOs.Invoice;

public class InvoiceDto
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public Guid ContractId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public string BreakdownJson { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public InvoiceStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
