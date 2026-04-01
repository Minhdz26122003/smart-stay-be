using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Announcement;
using SmartStay.Application.Interfaces;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Exceptions;
using SmartStay.Domain.Interfaces;

namespace SmartStay.Infrastructure.Services;

public class AnnouncementService(
    IRepository<Announcement> announcementRepository,
    IRepository<Property> propertyRepository,
    IRepository<Contract> contractRepository,
    IRepository<Room> roomRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IAnnouncementService
{
    public async Task<ApiResponse<AnnouncementDto>> CreateAnnouncementAsync(Guid landlordId, CreateAnnouncementRequest request)
    {
        var property = await propertyRepository.GetByIdAsync(request.PropertyId)
            ?? throw new NotFoundException(nameof(Property), request.PropertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to post announcements for this property.");

        var announcement = mapper.Map<Announcement>(request);
        announcement.CreatedBy = landlordId;

        await announcementRepository.AddAsync(announcement);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<AnnouncementDto>(announcement);
        var user = await userRepository.GetByIdAsync(landlordId);
        dto.CreatedByName = user?.FullName ?? "Ban Quản Lý";
        
        return ApiResponse<AnnouncementDto>.Ok(dto, "Announcement posted successfully.");
    }

    public async Task<ApiResponse<IEnumerable<AnnouncementDto>>> GetAnnouncementsByPropertyAsync(Guid propertyId)
    {
        var announcements = await announcementRepository.FindAsync(a => a.PropertyId == propertyId);
        var dtos = mapper.Map<IEnumerable<AnnouncementDto>>(announcements.OrderByDescending(a => a.CreatedAt)).ToList();
        
        foreach(var dto in dtos)
        {
            var user = await userRepository.GetByIdAsync(dto.CreatedBy);
            dto.CreatedByName = user?.FullName ?? "Ban Quản Lý";
        }
        
        return ApiResponse<IEnumerable<AnnouncementDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<IEnumerable<AnnouncementDto>>> GetAnnouncementsForTenantAsync(Guid tenantId)
    {
        // Lấy phòng của tenant qua hợp đồng đang hiệu lực
        var contracts = await contractRepository.FindAsync(c => c.TenantId == tenantId);
        var roomIds = contracts.Select(c => c.RoomId).Distinct().ToList();

        if (!roomIds.Any())
            return ApiResponse<IEnumerable<AnnouncementDto>>.Ok(Enumerable.Empty<AnnouncementDto>());

        var rooms = await roomRepository.FindAsync(r => roomIds.Contains(r.Id));
        var propertyIds = rooms.Select(r => r.PropertyId).Distinct().ToList();

        // Lấy thông báo: toàn khu trọ (RoomId == null) hoặc đúng phòng của tenant
        var announcements = await announcementRepository.FindAsync(a =>
            propertyIds.Contains(a.PropertyId) &&
            (a.RoomId == null || roomIds.Contains(a.RoomId.Value)));

        var dtos = mapper.Map<IEnumerable<AnnouncementDto>>(announcements.OrderByDescending(a => a.CreatedAt)).ToList();
        
        foreach(var dto in dtos)
        {
            var user = await userRepository.GetByIdAsync(dto.CreatedBy);
            dto.CreatedByName = user?.FullName ?? "Ban Quản Lý";
        }
        
        return ApiResponse<IEnumerable<AnnouncementDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<bool>> DeleteAnnouncementAsync(Guid landlordId, Guid announcementId)
    {
        var announcement = await announcementRepository.GetByIdAsync(announcementId)
            ?? throw new NotFoundException(nameof(Announcement), announcementId);

        var property = await propertyRepository.GetByIdAsync(announcement.PropertyId)
            ?? throw new NotFoundException(nameof(Property), announcement.PropertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to delete this announcement.");

        await announcementRepository.SoftDeleteAsync(announcementId);
        await unitOfWork.CommitAsync();

        return ApiResponse<bool>.Ok(true, "Announcement deleted successfully.");
    }
}
