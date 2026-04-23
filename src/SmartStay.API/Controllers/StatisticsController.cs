using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStay.Application.Interfaces;

namespace SmartStay.API.Controllers;

[Route("api/v1/statistics")]
[ApiController]
[Authorize(Roles = "Landlord")]
public class StatisticsController(IStatisticsService statisticsService) : ControllerBase
{
    private Guid GetUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedAccessException("Invalid user token.");
        return userId;
    }

    [HttpGet("finance-summary")]
    public async Task<IActionResult> GetFinanceSummary([FromQuery] int? month, [FromQuery] int? year)
    {
        var landlordId = GetUserId();
        var response = await statisticsService.GetFinanceSummaryAsync(landlordId, month, year);
        return Ok(response);
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard([FromQuery] Guid? propertyId)
    {
        var landlordId = GetUserId();
        var response = await statisticsService.GetLandlordDashboardAsync(landlordId, propertyId);
        return Ok(response);
    }
}
