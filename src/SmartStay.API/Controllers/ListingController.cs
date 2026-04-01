using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStay.Application.DTOs.Listing;
using SmartStay.Application.Interfaces;

namespace SmartStay.API.Controllers;

[Route("api/v1/listings")]
[ApiController]
[Authorize(Roles = "Landlord")]
public class ListingController(IListingService listingService) : ControllerBase
{
    private Guid GetLandlordId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedAccessException("Invalid user token.");
        return userId;
    }

    [HttpPost]
    public async Task<IActionResult> CreateListing([FromBody] CreateListingRequest request)
    {
        var landlordId = GetLandlordId();
        var response = await listingService.CreateListingAsync(landlordId, request);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetMyListings()
    {
        var landlordId = GetLandlordId();
        var response = await listingService.GetListingsByLandlordAsync(landlordId);
        return Ok(response);
    }

    [HttpPut("{id}/toggle")]
    public async Task<IActionResult> ToggleListing(Guid id)
    {
        var landlordId = GetLandlordId();
        var response = await listingService.ToggleListingActiveAsync(landlordId, id);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteListing(Guid id)
    {
        var landlordId = GetLandlordId();
        var response = await listingService.DeleteListingAsync(landlordId, id);
        return Ok(response);
    }
}
