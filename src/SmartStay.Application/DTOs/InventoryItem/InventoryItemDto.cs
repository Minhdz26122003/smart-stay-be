using System;
using System.Collections.Generic;
using SmartStay.Domain.Enums;

namespace SmartStay.Application.DTOs.InventoryItem;

public class InventoryItemDto
{
    public Guid Id { get; set; }
    public Guid ContractId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public List<string> CheckInPhotos { get; set; } = new();
    public List<string> CheckOutPhotos { get; set; } = new();
    public ItemCondition Condition { get; set; }
    public DateTime CreatedAt { get; set; }
}
