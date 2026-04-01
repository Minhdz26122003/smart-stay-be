using System;
using System.Collections.Generic;

namespace SmartStay.Application.DTOs.Listing;

public class CreateListingRequest
{
    public Guid RoomId { get; set; }
    public List<string> PhotoUrls { get; set; } = new();
    public string Description { get; set; } = string.Empty;
}
