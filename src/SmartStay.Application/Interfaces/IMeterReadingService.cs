using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.MeterReading;

namespace SmartStay.Application.Interfaces;

public interface IMeterReadingService
{
    Task<ApiResponse<MeterReadingDto>> CreateMeterReadingAsync(Guid landlordId, CreateMeterReadingRequest request);
    Task<ApiResponse<IEnumerable<MeterReadingDto>>> GetMeterReadingsByRoomAsync(Guid landlordId, Guid roomId);
    Task<ApiResponse<IEnumerable<MeterReadingDto>>> GetMeterReadingsByPropertyAsync(Guid landlordId, Guid propertyId, int month, int year);
    Task<ApiResponse<bool>> DeleteMeterReadingAsync(Guid landlordId, Guid id);
}
