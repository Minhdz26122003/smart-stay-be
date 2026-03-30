using System.Collections.Generic;

namespace SmartStay.Application.DTOs.Property;

public class CreatePropertyRequest
{
    public string Name { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string Ward { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? Rules { get; set; }
    public List<string> SharedAmenities { get; set; } = new();
}
