using SmartStay.Domain.Enums;

namespace SmartStay.Application.DTOs.Invoice;

public class UpdateInvoiceRequest
{
    public decimal PaidAmount { get; set; }
    public InvoiceStatus Status { get; set; }
}
