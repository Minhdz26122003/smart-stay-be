using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Roommate;

namespace SmartStay.Application.Interfaces;

public interface IRoommateService
{
    Task<ApiResponse<RoommateDto>> CreateRoommateAsync(Guid currentUserId, CreateRoommateRequest request);
    Task<ApiResponse<IEnumerable<RoommateDto>>> GetRoommatesByContractAsync(Guid currentUserId, Guid contractId);
    Task<ApiResponse<RoommateDto>> ApproveRoommateAsync(Guid landlordId, Guid roommateId);
    Task<ApiResponse<bool>> DeleteRoommateAsync(Guid currentUserId, Guid roommateId);
}
