using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Invoice;

namespace SmartStay.Application.Interfaces;

public interface IInvoiceService
{
    Task<ApiResponse<InvoiceDto>> CreateInvoiceAsync(Guid landlordId, CreateInvoiceRequest request);
    Task<ApiResponse<InvoiceDto>> UpdateInvoiceAsync(Guid landlordId, Guid invoiceId, UpdateInvoiceRequest request);
    Task<ApiResponse<InvoiceDto>> GetInvoiceByIdAsync(Guid invoiceId);
    Task<ApiResponse<IEnumerable<InvoiceDto>>> GetInvoicesByContractAsync(Guid contractId);
    Task<ApiResponse<bool>> DeleteInvoiceAsync(Guid landlordId, Guid invoiceId);
}
