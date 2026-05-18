using System;
using SmartStay.Domain.Enums;

namespace SmartStay.Domain.Entities;

public class Booking : BaseEntity
{
    public Guid ListingId { get; set; }
    public Guid GuestId { get; set; }
    public DateTime ProposedDateTime { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public string? Note { get; set; }

    public Listing? Listing { get; set; }
    public User? Guest { get; set; }
}
