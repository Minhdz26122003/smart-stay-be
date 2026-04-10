using System;

namespace SmartStay.Domain.Entities;

public class OtpCode : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string CodeHash { get; set; } = string.Empty; // BCrypt hash của mã 6 chữ số
    public string Purpose { get; set; } = string.Empty;  // "register" | "reset-password"
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; } = false;
}
