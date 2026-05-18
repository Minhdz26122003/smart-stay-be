using System;
using System.Collections.Generic;
using SmartStay.Domain.Enums;

namespace SmartStay.Domain.Entities;

public class InventoryItem : BaseEntity
{
    public Guid ContractId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public List<string> CheckInPhotos { get; set; } = new();
    public List<string> CheckOutPhotos { get; set; } = new();
    public ItemCondition Condition { get; set; } = ItemCondition.Good;

    public Contract? Contract { get; set; }
}
