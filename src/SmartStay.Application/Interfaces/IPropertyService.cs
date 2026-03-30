using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Property;

namespace SmartStay.Application.Interfaces;

public interface IPropertyService
{
    Task<ApiResponse<PropertyDto>> CreatePropertyAsync(Guid landlordId, CreatePropertyRequest request);
    Task<ApiResponse<PropertyDto>> UpdatePropertyAsync(Guid landlordId, Guid propertyId, UpdatePropertyRequest request);
    Task<ApiResponse<PropertyDto>> GetPropertyByIdAsync(Guid propertyId);
    Task<ApiResponse<IEnumerable<PropertyDto>>> GetPropertiesByLandlordAsync(Guid landlordId);
    Task<ApiResponse<bool>> DeletePropertyAsync(Guid landlordId, Guid propertyId);
}
