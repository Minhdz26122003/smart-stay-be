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
}
