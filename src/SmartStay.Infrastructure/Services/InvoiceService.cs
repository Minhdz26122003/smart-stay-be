using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Invoice;
using SmartStay.Application.Interfaces;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Exceptions;
using SmartStay.Domain.Interfaces;

namespace SmartStay.Infrastructure.Services;

public class InvoiceService(
    IRepository<Invoice> invoiceRepository,
    IRepository<Contract> contractRepository,
    IRepository<Room> roomRepository,
    IRepository<Property> propertyRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IInvoiceService
{
    private async Task VerifyLandlordOwnsRoomAsync(Guid landlordId, Guid roomId)
    {
        var room = await roomRepository.GetByIdAsync(roomId)
            ?? throw new NotFoundException(nameof(Room), roomId);

        var property = await propertyRepository.GetByIdAsync(room.PropertyId);
        if (property == null || property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to manage invoices for this room.");
    }

    private static void ValidateInvoicePeriod(int month, int year)
    {
        if (month is < 1 or > 12)
            throw new BadRequestException("Invoice month must be between 1 and 12.");

        if (year < 2000)
            throw new BadRequestException("Invoice year must be 2000 or later.");
    }

    private static void ValidateInvoiceAmounts(decimal totalAmount, decimal paidAmount)
    {
        if (totalAmount < 0)
            throw new BadRequestException("Invoice total amount cannot be negative.");

        if (paidAmount < 0)
            throw new BadRequestException("Invoice paid amount cannot be negative.");

        if (paidAmount > totalAmount)
            throw new BadRequestException("Invoice paid amount cannot exceed total amount.");
    }

    public async Task<ApiResponse<InvoiceDto>> CreateInvoiceAsync(Guid landlordId, CreateInvoiceRequest request)
    {
        ValidateInvoicePeriod(request.Month, request.Year);
        ValidateInvoiceAmounts(request.TotalAmount, 0);
        await VerifyLandlordOwnsRoomAsync(landlordId, request.RoomId);

        var contract = await contractRepository.GetByIdAsync(request.ContractId)
            ?? throw new NotFoundException(nameof(Contract), request.ContractId);

        if (contract.RoomId != request.RoomId)
            throw new BadRequestException("Invoice room does not match the contract room.");

        var existingInvoices = await invoiceRepository.FindAsync(i => i.ContractId == request.ContractId);
        if (existingInvoices.Any(i => i.Month == request.Month && i.Year == request.Year))
            throw new BadRequestException("An invoice already exists for this contract, month, and year.");

        var invoice = mapper.Map<Invoice>(request);
        await invoiceRepository.AddAsync(invoice);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<InvoiceDto>(invoice);
        return ApiResponse<InvoiceDto>.Ok(dto, "Invoice created successfully.");
    }

    public async Task<ApiResponse<bool>> DeleteInvoiceAsync(Guid landlordId, Guid invoiceId)
    {
        var invoice = await invoiceRepository.GetByIdAsync(invoiceId)
            ?? throw new NotFoundException(nameof(Invoice), invoiceId);

        await VerifyLandlordOwnsRoomAsync(landlordId, invoice.RoomId);

        await invoiceRepository.SoftDeleteAsync(invoiceId);
        await unitOfWork.CommitAsync();

        return ApiResponse<bool>.Ok(true, "Invoice deleted successfully.");
    }

    public async Task<ApiResponse<InvoiceDto>> GetInvoiceByIdAsync(Guid invoiceId)
    {
        var invoice = await invoiceRepository.GetByIdAsync(invoiceId)
            ?? throw new NotFoundException(nameof(Invoice), invoiceId);

        var dto = mapper.Map<InvoiceDto>(invoice);
        return ApiResponse<InvoiceDto>.Ok(dto);
    }

    public async Task<ApiResponse<IEnumerable<InvoiceDto>>> GetInvoicesByContractAsync(Guid contractId)
    {
        var invoices = await invoiceRepository.FindAsync(i => i.ContractId == contractId);
        var dtos = mapper.Map<IEnumerable<InvoiceDto>>(invoices);
        return ApiResponse<IEnumerable<InvoiceDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<InvoiceDto>> UpdateInvoiceAsync(Guid landlordId, Guid invoiceId, UpdateInvoiceRequest request)
    {
        var invoice = await invoiceRepository.GetByIdAsync(invoiceId)
            ?? throw new NotFoundException(nameof(Invoice), invoiceId);

        await VerifyLandlordOwnsRoomAsync(landlordId, invoice.RoomId);
        ValidateInvoiceAmounts(invoice.TotalAmount, request.PaidAmount);

        mapper.Map(request, invoice);
        invoiceRepository.Update(invoice);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<InvoiceDto>(invoice);
        return ApiResponse<InvoiceDto>.Ok(dto, "Invoice updated successfully.");
    }
}
