using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStay.Application.DTOs.ServiceConfig;
using SmartStay.Application.Interfaces;

namespace SmartStay.API.Controllers;

[Route("api/v1/service-configs")]
[ApiController]
[Authorize(Roles = "Landlord")]
public class ServiceConfigController(IServiceConfigService serviceConfigService) : ControllerBase
{
    private Guid GetLandlordId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedAccessException("Invalid user token.");
        return userId;
    }

    [HttpPost]
    public async Task<IActionResult> CreateServiceConfig([FromBody] CreateServiceConfigRequest request)
    {
        var landlordId = GetLandlordId();
        var response = await serviceConfigService.CreateServiceConfigAsync(landlordId, request);
        return Ok(response);
    }

    [HttpGet("property/{propertyId}")]
    public async Task<IActionResult> GetServiceConfigsByPropertyId(Guid propertyId)
    {
        var response = await serviceConfigService.GetServiceConfigsByPropertyIdAsync(propertyId);
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateServiceConfig(Guid id, [FromBody] UpdateServiceConfigRequest request)
    {
        var landlordId = GetLandlordId();
        var response = await serviceConfigService.UpdateServiceConfigAsync(landlordId, id, request);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteServiceConfig(Guid id)
    {
        var landlordId = GetLandlordId();
        var response = await serviceConfigService.DeleteServiceConfigAsync(landlordId, id);
        return Ok(response);
    }
}
