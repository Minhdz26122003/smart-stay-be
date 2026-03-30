using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Property;
using SmartStay.Application.Interfaces;

namespace SmartStay.API.Controllers;

[Route("api/v1/properties")]
[ApiController]
[Authorize(Roles = "Landlord")]
public class PropertyController(IPropertyService propertyService) : ControllerBase
{
    private Guid GetLandlordId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedAccessException("Invalid user token.");
        return userId;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyRequest request)
    {
        var landlordId = GetLandlordId();
        var response = await propertyService.CreatePropertyAsync(landlordId, request);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetMyProperties()
    {
        var landlordId = GetLandlordId();
        var response = await propertyService.GetPropertiesByLandlordAsync(landlordId);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPropertyById(Guid id)
    {
        var response = await propertyService.GetPropertyByIdAsync(id);
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProperty(Guid id, [FromBody] UpdatePropertyRequest request)
    {
        var landlordId = GetLandlordId();
        var response = await propertyService.UpdatePropertyAsync(landlordId, id, request);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProperty(Guid id)
    {
        var landlordId = GetLandlordId();
        var response = await propertyService.DeletePropertyAsync(landlordId, id);
        return Ok(response);
    }
}
