using System;
using System.Collections.Generic;

namespace SmartStay.Application.DTOs.Listing;

public class ListingDto
{
    public Guid Id { get; set; }
    public Guid LandlordId { get; set; }
    public Guid PropertyId { get; set; }
    public Guid? RoomId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public double Area { get; set; }
    public List<string> Facilities { get; set; } = new();
    public List<string> PhotoUrls { get; set; } = new();
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
