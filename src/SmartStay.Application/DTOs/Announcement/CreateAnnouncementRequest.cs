using System;

namespace SmartStay.Application.DTOs.Announcement;

public class CreateAnnouncementRequest
{
    public Guid PropertyId { get; set; }
    public Guid? RoomId { get; set; } // null = gửi toàn khu trọ
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
