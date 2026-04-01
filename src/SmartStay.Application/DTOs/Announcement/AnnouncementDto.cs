using System;

namespace SmartStay.Application.DTOs.Announcement;

public class AnnouncementDto
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public Guid? RoomId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid CreatedBy { get; set; }
    public string CreatedByName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
