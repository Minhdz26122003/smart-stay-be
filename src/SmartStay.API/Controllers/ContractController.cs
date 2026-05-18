using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStay.Application.DTOs.Contract;
using SmartStay.Application.Interfaces;

namespace SmartStay.API.Controllers;

[Route("api/v1/contracts")]
[ApiController]
[Authorize(Roles = "Landlord")] // Primarily managed by landlords
public class ContractController(IContractService contractService) : ControllerBase
{
    private Guid GetUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedAccessException("Invalid user token.");
        return userId;
    }

    [HttpPost]
    public async Task<IActionResult> CreateContract([FromBody] CreateContractRequest request)
    {
        var landlordId = GetUserId();
        var response = await contractService.CreateContractAsync(landlordId, request);
        return Ok(response);
    }

    [HttpGet("room/{roomId}")]
    public async Task<IActionResult> GetContractsByRoom(Guid roomId)
    {
        var response = await contractService.GetContractsByRoomAsync(roomId);
        return Ok(response);
    }

    [HttpGet("tenant/{tenantId}")]
    [AllowAnonymous] // Tenants should view their own contracts too, depending on requirements
    public async Task<IActionResult> GetContractsByTenant(Guid tenantId)
    {
        var response = await contractService.GetContractsByTenantAsync(tenantId);
        return Ok(response);
    }

    [HttpGet("property/{propertyId}")]
    public async Task<IActionResult> GetContractsByProperty(Guid propertyId)
    {
        var response = await contractService.GetContractsByPropertyAsync(propertyId);
        return Ok(response);
    }

    [HttpGet("landlord")]
    public async Task<IActionResult> GetContractsByLandlord()
    {
        var landlordId = GetUserId();
        var response = await contractService.GetContractsByLandlordAsync(landlordId);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetContractById(Guid id)
    {
        var response = await contractService.GetContractByIdAsync(id);
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContract(Guid id, [FromBody] UpdateContractRequest request)
    {
        var landlordId = GetUserId();
        var response = await contractService.UpdateContractAsync(landlordId, id, request);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContract(Guid id)
    {
        var landlordId = GetUserId();
        var response = await contractService.DeleteContractAsync(landlordId, id);
        return Ok(response);
    }
}
