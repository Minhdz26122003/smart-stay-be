using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStay.Application.DTOs.InventoryItem;
using SmartStay.Application.Interfaces;

namespace SmartStay.API.Controllers;

[Route("api/v1/inventory-items")]
[ApiController]
[Authorize(Roles = "Landlord")]
public class InventoryItemController(IInventoryItemService inventoryItemService) : ControllerBase
{
    private Guid GetLandlordId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedAccessException("Invalid user token.");
        return userId;
    }

    [HttpPost]
    public async Task<IActionResult> CreateInventoryItem([FromBody] CreateInventoryItemRequest request)
    {
        var landlordId = GetLandlordId();
        var response = await inventoryItemService.CreateInventoryItemAsync(landlordId, request);
        return Ok(response);
    }

    [HttpGet("contract/{contractId}")]
    public async Task<IActionResult> GetByContract(Guid contractId)
    {
        var response = await inventoryItemService.GetInventoryByContractAsync(contractId);
        return Ok(response);
    }

    [HttpPut("{id}/checkout")]
    public async Task<IActionResult> UpdateCheckout(Guid id, [FromBody] UpdateInventoryCheckoutRequest request)
    {
        var landlordId = GetLandlordId();
        var response = await inventoryItemService.UpdateCheckoutAsync(landlordId, id, request);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInventoryItem(Guid id)
    {
        var landlordId = GetLandlordId();
        var response = await inventoryItemService.DeleteInventoryItemAsync(landlordId, id);
        return Ok(response);
    }
}
