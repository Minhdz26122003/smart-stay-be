using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Auth;
using SmartStay.Application.Interfaces;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Exceptions;
using SmartStay.Domain.Interfaces;

namespace SmartStay.Infrastructure.Services;

public class AuthService(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IOtpCodeRepository otpCodeRepository,
    IEmailService emailService,
    IUnitOfWork unitOfWork,
    ITokenService tokenService,
    IMapper mapper) : IAuthService
{
    // ─── Register & Login ──────────────────────────────────────────────────────

    public async Task<ApiResponse<Guid>> RegisterAsync(RegisterRequest request)
    {
        if (await userRepository.ExistsByPhoneAsync(request.Phone))
            throw new ConflictException($"Phone number '{request.Phone}' is already registered.");

        if (!string.IsNullOrEmpty(request.Email) && await userRepository.ExistsByEmailAsync(request.Email))
            throw new ConflictException($"Email '{request.Email}' is already registered.");

        // Nếu có email, kiểm tra email đã được verify OTP chưa
        if (!string.IsNullOrEmpty(request.Email))
        {
            var hasVerifiedOtp = await otpCodeRepository.FindVerifiedRecentAsync(request.Email, "register");
            if (!hasVerifiedOtp)
                throw new BadRequestException("Email chưa được xác thực OTP. Vui lòng xác thực trước khi đăng ký.");
        }

        var user = new User
        {
            FullName = request.FullName,
            Phone = request.Phone,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Roles = [request.Role],
            IsEmailVerified = !string.IsNullOrEmpty(request.Email) // true nếu email đã verify qua OTP
        };

        await userRepository.AddAsync(user);
        await unitOfWork.CommitAsync();

        return ApiResponse<Guid>.Ok(user.Id, "Registration successful. Please login.");
    }

    public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var phoneOrEmail = request.PhoneOrEmail?.Trim() ?? string.Empty;
        var password = request.Password?.Trim() ?? string.Empty;

        User? user;
        if (phoneOrEmail.Contains('@'))
        {
            phoneOrEmail = phoneOrEmail.ToLowerInvariant();
            user = await userRepository.FindByEmailAsync(phoneOrEmail);
        }
        else
        {
            user = await userRepository.FindByPhoneAsync(phoneOrEmail);
        }

        if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new UnauthorizedException("Invalid credentials.");

        if (!user.IsActive)
            throw new UnauthorizedException("Account is disabled.");

        return await BuildAuthResponseAsync(user, request.DeviceInfo);
    }

    // ─── Token Management ──────────────────────────────────────────────────────

    public async Task<ApiResponse<AuthResponse>> RefreshTokenAsync(string rawToken)
    {
        var tokenHash = tokenService.HashToken(rawToken);
        var storedToken = await refreshTokenRepository.FindByTokenHashAsync(tokenHash);

        if (storedToken is null || !storedToken.IsActive)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        storedToken.IsRevoked = true;
        storedToken.UpdatedAt = DateTime.UtcNow;

        var user = await userRepository.GetByIdAsync(storedToken.UserId)
            ?? throw new NotFoundException(nameof(User), storedToken.UserId);

        await unitOfWork.CommitAsync();
        return await BuildAuthResponseAsync(user, storedToken.DeviceInfo);
    }

    public async Task<ApiResponse<bool>> RevokeTokenAsync(Guid userId)
    {
        await refreshTokenRepository.RevokeAllForUserAsync(userId);
        await unitOfWork.CommitAsync();
        return ApiResponse<bool>.Ok(true, "All sessions revoked.");
    }

    // ─── OTP: Registration ────────────────────────────────────────────────────

    public async Task<ApiResponse<bool>> SendRegistrationOtpAsync(string email)
    {
        var existingUser = await userRepository.FindByEmailAsync(email);
        if (existingUser != null && existingUser.IsEmailVerified)
            throw new ConflictException($"Email '{email}' already has a verified account.");

        await SendOtpAsync(email, "register");
        return ApiResponse<bool>.Ok(true, "OTP sent to email. Valid for 5 minutes.");
    }

    public async Task<ApiResponse<bool>> VerifyRegistrationOtpAsync(VerifyOtpRequest request)
    {
        await VerifyOtpCodeAsync(request.Email, request.Code, "register");

        // Nếu user đã tồn tại (đăng ký lại chưa xong), cập nhật verified
        var user = await userRepository.FindByEmailAsync(request.Email);
        if (user != null)
        {
            user.IsEmailVerified = true;
            await unitOfWork.CommitAsync();
        }

        return ApiResponse<bool>.Ok(true, "Email verified. You can now complete registration.");
    }

    // ─── OTP: Forgot Password ─────────────────────────────────────────────────

    public async Task<ApiResponse<bool>> SendForgotPasswordOtpAsync(string email)
    {
        var user = await userRepository.FindByEmailAsync(email)
            ?? throw new BadRequestException("Không tìm thấy tài khoản với email này.");

        await SendOtpAsync(email, "reset-password");
        return ApiResponse<bool>.Ok(true, "OTP sent to email. Valid for 5 minutes.");
    }

    public async Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequest request)
    {
        await VerifyOtpCodeAsync(request.Email, request.Code, "reset-password");

        var user = await userRepository.FindByEmailAsync(request.Email)
            ?? throw new BadRequestException("Không tìm thấy tài khoản.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;
        await unitOfWork.CommitAsync();

        return ApiResponse<bool>.Ok(true, "Password reset successfully. Please login.");
    }

    // ─── Private Helpers ──────────────────────────────────────────────────────

    private async Task SendOtpAsync(string email, string purpose)
    {
        var rawCode = new Random().Next(100000, 999999).ToString();
        var codeHash = BCrypt.Net.BCrypt.HashPassword(rawCode);

        await otpCodeRepository.InvalidateAllAsync(email, purpose);
        await otpCodeRepository.AddAsync(new OtpCode
        {
            Email = email,
            CodeHash = codeHash,
            Purpose = purpose,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5)
        });
        await unitOfWork.CommitAsync();

        var subject = purpose == "register"
            ? "Xác thực email SmartStay"
            : "Đặt lại mật khẩu SmartStay";

        var body = $@"
            <div style='font-family: Arial, sans-serif; max-width: 480px; margin: auto; padding: 24px; border: 1px solid #e0e0e0; border-radius: 8px;'>
                <h2 style='color: #2563eb;'>SmartStay</h2>
                <p>Mã OTP của bạn là:</p>
                <div style='font-size: 36px; font-weight: bold; letter-spacing: 8px; color: #1d4ed8; text-align: center; padding: 16px 0;'>{rawCode}</div>
                <p style='color: #6b7280;'>Mã có hiệu lực trong <strong>5 phút</strong>. Không chia sẻ mã này với bất kỳ ai.</p>
            </div>";

        await emailService.SendEmailAsync(email, subject, body);
    }

    private async Task VerifyOtpCodeAsync(string email, string code, string purpose)
    {
        var otp = await otpCodeRepository.FindLatestActiveAsync(email, purpose)
            ?? throw new BadRequestException("OTP không hợp lệ hoặc đã hết hạn.");

        if (!BCrypt.Net.BCrypt.Verify(code, otp.CodeHash))
            throw new BadRequestException("Mã OTP không đúng.");

        otp.IsUsed = true;
        otp.UpdatedAt = DateTime.UtcNow;
        await unitOfWork.CommitAsync();
    }

    private async Task<ApiResponse<AuthResponse>> BuildAuthResponseAsync(User user, string? deviceInfo)
    {
        var accessToken = tokenService.GenerateJwtToken(user);
        var (refreshEntity, rawRefreshToken) = tokenService.CreateRefreshToken(user, deviceInfo);

        await refreshTokenRepository.AddAsync(refreshEntity);
        await unitOfWork.CommitAsync();

        var response = mapper.Map<AuthResponse>(user);
        response.AccessToken = accessToken;
        response.RefreshToken = rawRefreshToken;
        response.AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(15);

        return ApiResponse<AuthResponse>.Ok(response, "Success");
    }
}
