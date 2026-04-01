using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Vehicle;

namespace SmartStay.Application.Interfaces;

public interface IVehicleService
{
    Task<ApiResponse<VehicleDto>> CreateVehicleAsync(Guid tenantId, CreateVehicleRequest request);
    Task<ApiResponse<IEnumerable<VehicleDto>>> GetVehiclesByTenantAsync(Guid tenantId);
    Task<ApiResponse<bool>> DeleteVehicleAsync(Guid tenantId, Guid vehicleId);
}
