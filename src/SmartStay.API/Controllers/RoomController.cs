using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Room;
using SmartStay.Application.Interfaces;

namespace SmartStay.API.Controllers;

[Route("api/v1/rooms")]
[ApiController]
[Authorize(Roles = "Landlord")] // We assume only Landlords can manage rooms primarily
public class RoomController(IRoomService roomService) : ControllerBase
{
    private Guid GetUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedAccessException("Invalid user token.");
        return userId;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest request)
    {
        var landlordId = GetUserId();
        var response = await roomService.CreateRoomAsync(landlordId, request);
        return Ok(response);
    }

    [HttpGet("property/{propertyId}")]
    [AllowAnonymous] // Anyone could view rooms of a property potentially, adjust as needed
    public async Task<IActionResult> GetRoomsByProperty(Guid propertyId)
    {
        var response = await roomService.GetRoomsByPropertyAsync(propertyId);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [AllowAnonymous] // Anyone could view room details potentially
    public async Task<IActionResult> GetRoomById(Guid id)
    {
        var response = await roomService.GetRoomByIdAsync(id);
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoom(Guid id, [FromBody] UpdateRoomRequest request)
    {
        var landlordId = GetUserId();
        var response = await roomService.UpdateRoomAsync(landlordId, id, request);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(Guid id)
    {
        var landlordId = GetUserId();
        var response = await roomService.DeleteRoomAsync(landlordId, id);
        return Ok(response);
    }
}
