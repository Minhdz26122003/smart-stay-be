using System;
using System.Collections.Generic;

namespace SmartStay.Domain.Entities;

public class Listing : BaseEntity
{
    public Guid RoomId { get; set; }
    public List<string> PhotoUrls { get; set; } = new();
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
