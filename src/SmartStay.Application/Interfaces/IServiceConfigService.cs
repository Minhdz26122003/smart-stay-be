using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.ServiceConfig;

namespace SmartStay.Application.Interfaces;

public interface IServiceConfigService
{
    Task<ApiResponse<ServiceConfigDto>> CreateServiceConfigAsync(Guid landlordId, CreateServiceConfigRequest request);
    Task<ApiResponse<ServiceConfigDto>> UpdateServiceConfigAsync(Guid landlordId, Guid configId, UpdateServiceConfigRequest request);
    Task<ApiResponse<IEnumerable<ServiceConfigDto>>> GetServiceConfigsByPropertyIdAsync(Guid propertyId);
    Task<ApiResponse<bool>> DeleteServiceConfigAsync(Guid landlordId, Guid configId);
}
