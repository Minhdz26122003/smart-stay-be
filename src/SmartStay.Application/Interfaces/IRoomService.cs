using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Room;

namespace SmartStay.Application.Interfaces;

public interface IRoomService
{
    Task<ApiResponse<RoomDto>> CreateRoomAsync(Guid landlordId, CreateRoomRequest request);
    Task<ApiResponse<RoomDto>> UpdateRoomAsync(Guid landlordId, Guid roomId, UpdateRoomRequest request);
    Task<ApiResponse<RoomDto>> GetRoomByIdAsync(Guid roomId);
    Task<ApiResponse<IEnumerable<RoomDto>>> GetRoomsByPropertyAsync(Guid propertyId);
    Task<ApiResponse<bool>> DeleteRoomAsync(Guid landlordId, Guid roomId);
}
