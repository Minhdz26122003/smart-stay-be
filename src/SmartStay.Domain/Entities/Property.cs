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

    public User? Landlord { get; set; }
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
    public ICollection<ServiceConfig> ServiceConfigs { get; set; } = new List<ServiceConfig>();
    public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
}
