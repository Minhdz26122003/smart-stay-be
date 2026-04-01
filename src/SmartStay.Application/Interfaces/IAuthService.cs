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
}
