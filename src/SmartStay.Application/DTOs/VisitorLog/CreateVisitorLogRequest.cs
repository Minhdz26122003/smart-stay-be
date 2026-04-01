using System;

namespace SmartStay.Application.DTOs.VisitorLog;

public class CreateVisitorLogRequest
{
    public string VisitorName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool StayOvernight { get; set; }
}
