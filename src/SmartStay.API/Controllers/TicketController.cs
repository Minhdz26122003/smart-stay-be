using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStay.Application.DTOs.Ticket;
using SmartStay.Application.Interfaces;

namespace SmartStay.API.Controllers;

[Route("api/v1/tickets")]
[ApiController]
public class TicketController(ITicketService ticketService) : ControllerBase
{
    private Guid GetUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedAccessException("Invalid user token.");
        return userId;
    }

    [HttpPost]
    [Authorize(Roles = "Tenant")]
    public async Task<IActionResult> CreateTicket([FromBody] CreateTicketRequest request)
    {
        var tenantId = GetUserId();
        var response = await ticketService.CreateTicketAsync(tenantId, request);
        return Ok(response);
    }

    [HttpGet("tenant")]
    [Authorize(Roles = "Tenant")]
    public async Task<IActionResult> GetMyTicketsAsTenant()
    {
        var tenantId = GetUserId();
        var response = await ticketService.GetTicketsByTenantAsync(tenantId);
        return Ok(response);
    }

    [HttpGet("landlord")]
    [Authorize(Roles = "Landlord")]
    public async Task<IActionResult> GetMyTicketsAsLandlord()
    {
        var landlordId = GetUserId();
        var response = await ticketService.GetTicketsByLandlordAsync(landlordId);
        return Ok(response);
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Landlord")]
    public async Task<IActionResult> UpdateTicketStatus(Guid id, [FromBody] UpdateTicketStatusRequest request)
    {
        var landlordId = GetUserId();
        var response = await ticketService.UpdateTicketStatusAsync(landlordId, id, request);
        return Ok(response);
    }
}
