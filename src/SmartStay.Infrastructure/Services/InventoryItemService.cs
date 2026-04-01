using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.InventoryItem;
using SmartStay.Application.Interfaces;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Exceptions;
using SmartStay.Domain.Interfaces;

namespace SmartStay.Infrastructure.Services;

public class InventoryItemService(
    IRepository<InventoryItem> inventoryRepository,
    IRepository<Contract> contractRepository,
    IRepository<Room> roomRepository,
    IRepository<Property> propertyRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IInventoryItemService
{
    public async Task<ApiResponse<InventoryItemDto>> CreateInventoryItemAsync(Guid landlordId, CreateInventoryItemRequest request)
    {
        var contract = await contractRepository.GetByIdAsync(request.ContractId)
            ?? throw new NotFoundException(nameof(Contract), request.ContractId);

        var room = await roomRepository.GetByIdAsync(contract.RoomId)
            ?? throw new NotFoundException(nameof(Room), contract.RoomId);

        var property = await propertyRepository.GetByIdAsync(room.PropertyId)
            ?? throw new NotFoundException(nameof(Property), room.PropertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to add inventory for this contract.");

        var item = mapper.Map<InventoryItem>(request);
        await inventoryRepository.AddAsync(item);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<InventoryItemDto>(item);
        return ApiResponse<InventoryItemDto>.Ok(dto, "Inventory item added successfully.");
    }

    public async Task<ApiResponse<IEnumerable<InventoryItemDto>>> GetInventoryByContractAsync(Guid contractId)
    {
        var items = await inventoryRepository.FindAsync(i => i.ContractId == contractId);
        var dtos = mapper.Map<IEnumerable<InventoryItemDto>>(items.OrderBy(i => i.CreatedAt));
        return ApiResponse<IEnumerable<InventoryItemDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<InventoryItemDto>> UpdateCheckoutAsync(Guid landlordId, Guid itemId, UpdateInventoryCheckoutRequest request)
    {
        var item = await inventoryRepository.GetByIdAsync(itemId)
            ?? throw new NotFoundException(nameof(InventoryItem), itemId);

        var contract = await contractRepository.GetByIdAsync(item.ContractId)
            ?? throw new NotFoundException(nameof(Contract), item.ContractId);

        var room = await roomRepository.GetByIdAsync(contract.RoomId)
            ?? throw new NotFoundException(nameof(Room), contract.RoomId);

        var property = await propertyRepository.GetByIdAsync(room.PropertyId)
            ?? throw new NotFoundException(nameof(Property), room.PropertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to update this inventory item.");

        item.CheckOutPhotos = request.CheckOutPhotos;
        item.Condition = request.Condition;
        inventoryRepository.Update(item);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<InventoryItemDto>(item);
        return ApiResponse<InventoryItemDto>.Ok(dto, "Checkout information updated successfully.");
    }

    public async Task<ApiResponse<bool>> DeleteInventoryItemAsync(Guid landlordId, Guid itemId)
    {
        var item = await inventoryRepository.GetByIdAsync(itemId)
            ?? throw new NotFoundException(nameof(InventoryItem), itemId);

        var contract = await contractRepository.GetByIdAsync(item.ContractId)
            ?? throw new NotFoundException(nameof(Contract), item.ContractId);

        var room = await roomRepository.GetByIdAsync(contract.RoomId)
            ?? throw new NotFoundException(nameof(Room), contract.RoomId);

        var property = await propertyRepository.GetByIdAsync(room.PropertyId)
            ?? throw new NotFoundException(nameof(Property), room.PropertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to delete this inventory item.");

        await inventoryRepository.SoftDeleteAsync(itemId);
        await unitOfWork.CommitAsync();

        return ApiResponse<bool>.Ok(true, "Inventory item deleted successfully.");
    }
}
