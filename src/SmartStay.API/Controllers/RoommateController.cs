using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStay.Application.DTOs.Roommate;
using SmartStay.Application.Interfaces;

namespace SmartStay.API.Controllers;

[Route("api/v1/roommates")]
[ApiController]
[Authorize]
public class RoommateController(IRoommateService roommateService) : ControllerBase
{
    private Guid GetUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedAccessException("Invalid user token.");
        return userId;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoommate([FromBody] CreateRoommateRequest request)
    {
        var currentUserId = GetUserId();
        var response = await roommateService.CreateRoommateAsync(currentUserId, request);
        return Ok(response);
    }

    [HttpGet("contract/{contractId}")]
    public async Task<IActionResult> GetRoommatesByContract(Guid contractId)
    {
        var currentUserId = GetUserId();
        var response = await roommateService.GetRoommatesByContractAsync(currentUserId, contractId);
        return Ok(response);
    }

    [HttpPut("{id}/approve")]
    [Authorize(Roles = "Landlord")]
    public async Task<IActionResult> ApproveRoommate(Guid id)
    {
        var landlordId = GetUserId();
        var response = await roommateService.ApproveRoommateAsync(landlordId, id);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoommate(Guid id)
    {
        var currentUserId = GetUserId();
        var response = await roommateService.DeleteRoommateAsync(currentUserId, id);
        return Ok(response);
    }
}
