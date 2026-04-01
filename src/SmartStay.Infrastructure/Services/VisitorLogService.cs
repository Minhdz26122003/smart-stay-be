using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.VisitorLog;
using SmartStay.Application.Interfaces;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Exceptions;
using SmartStay.Domain.Interfaces;

namespace SmartStay.Infrastructure.Services;

public class VisitorLogService(
    IRepository<VisitorLog> visitorLogRepository,
    IRepository<Contract> contractRepository,
    IRepository<Room> roomRepository,
    IRepository<Property> propertyRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IVisitorLogService
{
    public async Task<ApiResponse<VisitorLogDto>> CreateVisitorLogAsync(Guid tenantId, CreateVisitorLogRequest request)
    {
        var log = mapper.Map<VisitorLog>(request);
        log.TenantId = tenantId;
        log.ArrivedAt = DateTime.UtcNow;

        await visitorLogRepository.AddAsync(log);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<VisitorLogDto>(log);
        return ApiResponse<VisitorLogDto>.Ok(dto, "Visitor logged successfully.");
    }

    public async Task<ApiResponse<IEnumerable<VisitorLogDto>>> GetVisitorLogsByTenantAsync(Guid tenantId)
    {
        var logs = await visitorLogRepository.FindAsync(v => v.TenantId == tenantId);
        var dtos = mapper.Map<IEnumerable<VisitorLogDto>>(logs.OrderByDescending(v => v.ArrivedAt));
        return ApiResponse<IEnumerable<VisitorLogDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<IEnumerable<VisitorLogDto>>> GetVisitorLogsByLandlordAsync(Guid landlordId)
    {
        // 1. Get properties of landlord
        var properties = await propertyRepository.FindAsync(p => p.LandlordId == landlordId);
        var propertyIds = properties.Select(p => p.Id).ToList();

        // 2. Get rooms in properties
        var rooms = await roomRepository.FindAsync(r => propertyIds.Contains(r.PropertyId));
        var roomIds = rooms.Select(r => r.Id).ToList();

        // 3. Get contracts (tenants) in rooms
        var contracts = await contractRepository.FindAsync(c => roomIds.Contains(c.RoomId));
        var tenantIds = contracts.Select(c => c.TenantId).Distinct().ToList();

        // 4. Get logs of those tenants
        var logs = await visitorLogRepository.FindAsync(v => tenantIds.Contains(v.TenantId));
        var dtos = mapper.Map<IEnumerable<VisitorLogDto>>(logs.OrderByDescending(v => v.ArrivedAt));

        return ApiResponse<IEnumerable<VisitorLogDto>>.Ok(dtos);
    }
}
