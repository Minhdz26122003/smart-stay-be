namespace SmartStay.Application.DTOs.Auth;

public class LoginRequest
{
    public string PhoneOrEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? DeviceInfo { get; set; }
}
