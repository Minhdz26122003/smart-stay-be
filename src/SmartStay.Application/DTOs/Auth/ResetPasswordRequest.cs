namespace SmartStay.Application.DTOs.Auth;

public class ResetPasswordRequest
{
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;        // OTP xác nhận lần cuối
    public string NewPassword { get; set; } = string.Empty;
}
