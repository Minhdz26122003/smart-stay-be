using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.InventoryItem;

namespace SmartStay.Application.Interfaces;

public interface IInventoryItemService
{
    Task<ApiResponse<InventoryItemDto>> CreateInventoryItemAsync(Guid landlordId, CreateInventoryItemRequest request);
    Task<ApiResponse<IEnumerable<InventoryItemDto>>> GetInventoryByContractAsync(Guid contractId);
    Task<ApiResponse<InventoryItemDto>> UpdateCheckoutAsync(Guid landlordId, Guid itemId, UpdateInventoryCheckoutRequest request);
    Task<ApiResponse<bool>> DeleteInventoryItemAsync(Guid landlordId, Guid itemId);
}
