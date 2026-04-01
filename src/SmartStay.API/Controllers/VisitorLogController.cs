using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStay.Application.DTOs.VisitorLog;
using SmartStay.Application.Interfaces;

namespace SmartStay.API.Controllers;

[Route("api/v1/visitor-logs")]
[ApiController]
public class VisitorLogController(IVisitorLogService visitorLogService) : ControllerBase
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
    public async Task<IActionResult> CreateVisitorLog([FromBody] CreateVisitorLogRequest request)
    {
        var tenantId = GetUserId();
        var response = await visitorLogService.CreateVisitorLogAsync(tenantId, request);
        return Ok(response);
    }

    [HttpGet("tenant")]
    [Authorize(Roles = "Tenant")]
    public async Task<IActionResult> GetMyVisitorLogs()
    {
        var tenantId = GetUserId();
        var response = await visitorLogService.GetVisitorLogsByTenantAsync(tenantId);
        return Ok(response);
    }

    [HttpGet("landlord")]
    [Authorize(Roles = "Landlord")]
    public async Task<IActionResult> GetPropertyVisitorLogs()
    {
        var landlordId = GetUserId();
        var response = await visitorLogService.GetVisitorLogsByLandlordAsync(landlordId);
        return Ok(response);
    }
}
