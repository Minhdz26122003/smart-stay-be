using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Contract;

namespace SmartStay.Application.Interfaces;

public interface IContractService
{
    Task<ApiResponse<ContractDto>> CreateContractAsync(Guid landlordId, CreateContractRequest request);
    Task<ApiResponse<ContractDto>> UpdateContractAsync(Guid landlordId, Guid contractId, UpdateContractRequest request);
    Task<ApiResponse<ContractDto>> GetContractByIdAsync(Guid contractId);
    Task<ApiResponse<IEnumerable<ContractDto>>> GetContractsByRoomAsync(Guid roomId);
    Task<ApiResponse<IEnumerable<ContractDto>>> GetContractsByTenantAsync(Guid tenantId);
    Task<ApiResponse<IEnumerable<ContractDto>>> GetContractsByPropertyAsync(Guid propertyId);
    Task<ApiResponse<IEnumerable<ContractDto>>> GetContractsByLandlordAsync(Guid landlordId);
    Task<ApiResponse<bool>> DeleteContractAsync(Guid landlordId, Guid contractId);
}
