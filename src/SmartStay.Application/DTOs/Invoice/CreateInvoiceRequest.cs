using System;

namespace SmartStay.Application.DTOs.Invoice;

public class CreateInvoiceRequest
{
    public Guid RoomId { get; set; }
    public Guid ContractId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public string BreakdownJson { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}
