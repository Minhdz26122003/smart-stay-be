using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Vehicle;
using SmartStay.Application.Interfaces;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Exceptions;
using SmartStay.Domain.Interfaces;

namespace SmartStay.Infrastructure.Services;

public class VehicleService(
    IRepository<Vehicle> vehicleRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IVehicleService
{
    public async Task<ApiResponse<VehicleDto>> CreateVehicleAsync(Guid tenantId, CreateVehicleRequest request)
    {
        var vehicle = mapper.Map<Vehicle>(request);
        vehicle.TenantId = tenantId;

        await vehicleRepository.AddAsync(vehicle);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<VehicleDto>(vehicle);
        return ApiResponse<VehicleDto>.Ok(dto, "Vehicle registered successfully.");
    }

    public async Task<ApiResponse<IEnumerable<VehicleDto>>> GetVehiclesByTenantAsync(Guid tenantId)
    {
        var vehicles = await vehicleRepository.FindAsync(v => v.TenantId == tenantId);
        var dtos = mapper.Map<IEnumerable<VehicleDto>>(vehicles.OrderByDescending(v => v.CreatedAt));
        return ApiResponse<IEnumerable<VehicleDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<bool>> DeleteVehicleAsync(Guid tenantId, Guid vehicleId)
    {
        var vehicle = await vehicleRepository.GetByIdAsync(vehicleId)
            ?? throw new NotFoundException(nameof(Vehicle), vehicleId);

        if (vehicle.TenantId != tenantId)
            throw new UnauthorizedException("You do not have permission to delete this vehicle.");

        await vehicleRepository.SoftDeleteAsync(vehicleId);
        await unitOfWork.CommitAsync();

        return ApiResponse<bool>.Ok(true, "Vehicle removed successfully.");
    }
}
