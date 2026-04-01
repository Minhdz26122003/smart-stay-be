using System;
using System.Collections.Generic;

namespace SmartStay.Application.DTOs.Listing;

public class ListingDto
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public List<string> PhotoUrls { get; set; } = new();
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
