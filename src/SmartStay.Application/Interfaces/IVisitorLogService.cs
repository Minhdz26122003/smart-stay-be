using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.VisitorLog;

namespace SmartStay.Application.Interfaces;

public interface IVisitorLogService
{
    Task<ApiResponse<VisitorLogDto>> CreateVisitorLogAsync(Guid tenantId, CreateVisitorLogRequest request);
    Task<ApiResponse<IEnumerable<VisitorLogDto>>> GetVisitorLogsByTenantAsync(Guid tenantId);
    Task<ApiResponse<IEnumerable<VisitorLogDto>>> GetVisitorLogsByLandlordAsync(Guid landlordId);
}
