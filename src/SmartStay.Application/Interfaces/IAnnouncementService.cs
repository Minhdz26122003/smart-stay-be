using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Announcement;

namespace SmartStay.Application.Interfaces;

public interface IAnnouncementService
{
    Task<ApiResponse<AnnouncementDto>> CreateAnnouncementAsync(Guid landlordId, CreateAnnouncementRequest request);
    Task<ApiResponse<IEnumerable<AnnouncementDto>>> GetAnnouncementsByPropertyAsync(Guid propertyId);
    Task<ApiResponse<IEnumerable<AnnouncementDto>>> GetAnnouncementsForTenantAsync(Guid tenantId);
    Task<ApiResponse<bool>> DeleteAnnouncementAsync(Guid landlordId, Guid announcementId);
}
