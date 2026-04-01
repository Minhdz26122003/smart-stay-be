using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStay.Application.DTOs.Vehicle;
using SmartStay.Application.Interfaces;

namespace SmartStay.API.Controllers;

[Route("api/v1/vehicles")]
[ApiController]
[Authorize(Roles = "Tenant")]
public class VehicleController(IVehicleService vehicleService) : ControllerBase
{
    private Guid GetTenantId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedAccessException("Invalid user token.");
        return userId;
    }

    [HttpPost]
    public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequest request)
    {
        var tenantId = GetTenantId();
        var response = await vehicleService.CreateVehicleAsync(tenantId, request);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetMyVehicles()
    {
        var tenantId = GetTenantId();
        var response = await vehicleService.GetVehiclesByTenantAsync(tenantId);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVehicle(Guid id)
    {
        var tenantId = GetTenantId();
        var response = await vehicleService.DeleteVehicleAsync(tenantId, id);
        return Ok(response);
    }
}
