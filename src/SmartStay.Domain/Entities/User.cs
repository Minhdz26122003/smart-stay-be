using System;
using System.Collections.Generic;

namespace SmartStay.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public string? FcmToken { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsEmailVerified { get; set; } = false;

    public ICollection<Property> PropertiesAsLandlord { get; set; } = new List<Property>();
    public ICollection<Contract> ContractsAsTenant { get; set; } = new List<Contract>();
    public ICollection<Booking> BookingsAsGuest { get; set; } = new List<Booking>();
    public ICollection<Ticket> TicketsAsTenant { get; set; } = new List<Ticket>();
    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    public ICollection<VisitorLog> VisitorLogs { get; set; } = new List<VisitorLog>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<Review> ReviewsWritten { get; set; } = new List<Review>();
    public ICollection<Review> ReviewsReceived { get; set; } = new List<Review>();
    public ICollection<Announcement> AnnouncementsCreated { get; set; } = new List<Announcement>();
    public ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
}
