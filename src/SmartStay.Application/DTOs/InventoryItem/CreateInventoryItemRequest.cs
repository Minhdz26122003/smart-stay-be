using System;
using System.Collections.Generic;
using SmartStay.Domain.Enums;

namespace SmartStay.Application.DTOs.InventoryItem;

public class CreateInventoryItemRequest
{
    public Guid ContractId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public List<string> CheckInPhotos { get; set; } = new();
    public ItemCondition Condition { get; set; } = ItemCondition.Good;
}
