namespace SmartStay.Application.DTOs.Auth;

public class VerifyOtpRequest
{
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty; // "register" | "reset-password"
}
