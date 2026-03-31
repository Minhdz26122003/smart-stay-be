using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStay.Application.DTOs.MeterReading;
using SmartStay.Application.Interfaces;

namespace SmartStay.API.Controllers;

[Route("api/v1/meter-readings")]
[ApiController]
[Authorize(Roles = "Landlord")]
public class MeterReadingController(IMeterReadingService meterReadingService) : ControllerBase
{
    private Guid GetLandlordId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedAccessException("Invalid user token.");
        return userId;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMeterReading([FromBody] CreateMeterReadingRequest request)
    {
        var landlordId = GetLandlordId();
        var response = await meterReadingService.CreateMeterReadingAsync(landlordId, request);
        return Ok(response);
    }

    [HttpGet("room/{roomId}")]
    public async Task<IActionResult> GetMeterReadingsByRoom(Guid roomId)
    {
        var landlordId = GetLandlordId();
        var response = await meterReadingService.GetMeterReadingsByRoomAsync(landlordId, roomId);
        return Ok(response);
    }

    [HttpGet("property/{propertyId}")]
    public async Task<IActionResult> GetMeterReadingsByProperty(Guid propertyId, [FromQuery] int month, [FromQuery] int year)
    {
        var landlordId = GetLandlordId();
        var response = await meterReadingService.GetMeterReadingsByPropertyAsync(landlordId, propertyId, month, year);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMeterReading(Guid id)
    {
        var landlordId = GetLandlordId();
        var response = await meterReadingService.DeleteMeterReadingAsync(landlordId, id);
        return Ok(response);
    }
}
