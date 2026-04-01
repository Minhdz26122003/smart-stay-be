using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStay.Application.DTOs.Announcement;
using SmartStay.Application.Interfaces;

namespace SmartStay.API.Controllers;

[Route("api/v1/announcements")]
[ApiController]
public class AnnouncementController(IAnnouncementService announcementService) : ControllerBase
{
    private Guid GetUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedAccessException("Invalid user token.");
        return userId;
    }

    [HttpPost]
    [Authorize(Roles = "Landlord")]
    public async Task<IActionResult> CreateAnnouncement([FromBody] CreateAnnouncementRequest request)
    {
        var landlordId = GetUserId();
        var response = await announcementService.CreateAnnouncementAsync(landlordId, request);
        return Ok(response);
    }

    [HttpGet("property/{propertyId}")]
    [Authorize(Roles = "Landlord")]
    public async Task<IActionResult> GetByProperty(Guid propertyId)
    {
        var response = await announcementService.GetAnnouncementsByPropertyAsync(propertyId);
        return Ok(response);
    }

    [HttpGet("tenant")]
    [Authorize(Roles = "Tenant")]
    public async Task<IActionResult> GetMyAnnouncements()
    {
        var tenantId = GetUserId();
        var response = await announcementService.GetAnnouncementsForTenantAsync(tenantId);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Landlord")]
    public async Task<IActionResult> DeleteAnnouncement(Guid id)
    {
        var landlordId = GetUserId();
        var response = await announcementService.DeleteAnnouncementAsync(landlordId, id);
        return Ok(response);
    }
}
