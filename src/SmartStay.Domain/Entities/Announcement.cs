using System;

namespace SmartStay.Domain.Entities;

public class Announcement : BaseEntity
{
    public Guid PropertyId { get; set; }
    public Guid? RoomId { get; set; } // Null if it's for the whole property
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid CreatedBy { get; set; }
}
