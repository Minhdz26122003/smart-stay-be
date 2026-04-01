using System;
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
    IUnitOfWork unitOfWork,
    ITokenService tokenService,
    IMapper mapper) : IAuthService
{
    public async Task<ApiResponse<Guid>> RegisterAsync(RegisterRequest request)
    {
        if (await userRepository.ExistsByPhoneAsync(request.Phone))
            throw new ConflictException($"Phone number '{request.Phone}' is already registered.");

        if (!string.IsNullOrEmpty(request.Email) && await userRepository.ExistsByEmailAsync(request.Email))
            throw new ConflictException($"Email '{request.Email}' is already registered.");

        var user = new User
        {
            FullName = request.FullName,
            Phone = request.Phone,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Roles = [request.Role]
        };

        await userRepository.AddAsync(user);
        await unitOfWork.CommitAsync();

        return ApiResponse<Guid>.Ok(user.Id, "Registration successful. Please login.");
    }

    public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request)
    {
        User? user;
        if (request.PhoneOrEmail.Contains('@'))
            user = await userRepository.FindByEmailAsync(request.PhoneOrEmail);
        else
            user = await userRepository.FindByPhoneAsync(request.PhoneOrEmail);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid credentials.");

        if (!user.IsActive)
            throw new UnauthorizedException("Account is disabled.");

        return await BuildAuthResponseAsync(user, request.DeviceInfo);
    }

    public async Task<ApiResponse<AuthResponse>> RefreshTokenAsync(string rawToken)
    {
        var tokenHash = tokenService.HashToken(rawToken);
        var storedToken = await refreshTokenRepository.FindByTokenHashAsync(tokenHash);

        if (storedToken is null || !storedToken.IsActive)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        // Revoke old token (Refresh Token Rotation)
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

    private async Task<ApiResponse<AuthResponse>> BuildAuthResponseAsync(User user, string? deviceInfo)
    {
        var accessToken = tokenService.GenerateJwtToken(user);
        var (refreshEntity, rawRefreshToken) = tokenService.CreateRefreshToken(user, deviceInfo);

        await refreshTokenRepository.AddAsync(refreshEntity);
        await unitOfWork.CommitAsync();

        var response = mapper.Map<AuthResponse>(user);
        response.AccessToken = accessToken;
        response.RefreshToken = rawRefreshToken; // Raw token sent to client
        response.AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(15);

        return ApiResponse<AuthResponse>.Ok(response, "Success");
    }
}
