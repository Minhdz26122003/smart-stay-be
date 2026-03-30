using System;
using System.Collections.Generic;
using SmartStay.Domain.ValueObjects;

namespace SmartStay.Domain.Entities;

public class Property : BaseEntity
{
    public Guid LandlordId { get; set; }
    public string Name { get; set; } = string.Empty;
    public Address? Address { get; set; }
    public string? Rules { get; set; }
    public List<string> SharedAmenities { get; set; } = new();
}
