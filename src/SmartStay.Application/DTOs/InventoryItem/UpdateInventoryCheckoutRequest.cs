using System;
using System.Collections.Generic;
using SmartStay.Domain.Enums;

namespace SmartStay.Application.DTOs.InventoryItem;

public class UpdateInventoryCheckoutRequest
{
    public List<string> CheckOutPhotos { get; set; } = new();
    public ItemCondition Condition { get; set; }
}
