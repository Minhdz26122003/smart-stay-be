using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStay.Application.DTOs.Invoice;
using SmartStay.Application.Interfaces;

namespace SmartStay.API.Controllers;

[Route("api/v1/invoices")]
[ApiController]
[Authorize(Roles = "Landlord")]
public class InvoiceController(IInvoiceService invoiceService) : ControllerBase
{
    private Guid GetUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedAccessException("Invalid user token.");
        return userId;
    }

    [HttpPost]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceRequest request)
    {
        var landlordId = GetUserId();
        var response = await invoiceService.CreateInvoiceAsync(landlordId, request);
        return Ok(response);
    }

    [HttpGet("contract/{contractId}")]
    [AllowAnonymous] // Tenants might view invoices
    public async Task<IActionResult> GetInvoicesByContract(Guid contractId)
    {
        var response = await invoiceService.GetInvoicesByContractAsync(contractId);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetInvoiceById(Guid id)
    {
        var response = await invoiceService.GetInvoiceByIdAsync(id);
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInvoice(Guid id, [FromBody] UpdateInvoiceRequest request)
    {
        var landlordId = GetUserId();
        var response = await invoiceService.UpdateInvoiceAsync(landlordId, id, request);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInvoice(Guid id)
    {
        var landlordId = GetUserId();
        var response = await invoiceService.DeleteInvoiceAsync(landlordId, id);
        return Ok(response);
    }
}
