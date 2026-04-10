using System;
using System.Threading.Tasks;
using SmartStay.Application.DTOs.Auth;
using SmartStay.Application.Common.Models;

namespace SmartStay.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request);
    Task<ApiResponse<Guid>> RegisterAsync(RegisterRequest request);
    Task<ApiResponse<AuthResponse>> RefreshTokenAsync(string token);
    Task<ApiResponse<bool>> RevokeTokenAsync(Guid userId);

    // OTP flows
    Task<ApiResponse<bool>> SendRegistrationOtpAsync(string email);
    Task<ApiResponse<bool>> VerifyRegistrationOtpAsync(VerifyOtpRequest request);
    Task<ApiResponse<bool>> SendForgotPasswordOtpAsync(string email);
    Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequest request);
}
