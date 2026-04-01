using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.MeterReading;
using SmartStay.Application.Interfaces;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Exceptions;
using SmartStay.Domain.Interfaces;

namespace SmartStay.Infrastructure.Services;

public class MeterReadingService(
    IRepository<MeterReading> meterReadingRepository,
    IRepository<Room> roomRepository,
    IRepository<Property> propertyRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IMeterReadingService
{
    public async Task<ApiResponse<MeterReadingDto>> CreateMeterReadingAsync(Guid landlordId, CreateMeterReadingRequest request)
    {
        var room = await roomRepository.GetByIdAsync(request.RoomId)
            ?? throw new NotFoundException(nameof(Room), request.RoomId);

        var property = await propertyRepository.GetByIdAsync(room.PropertyId)
            ?? throw new NotFoundException(nameof(Property), room.PropertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to add meter reading for this room.");

        var meterReading = mapper.Map<MeterReading>(request);
        await meterReadingRepository.AddAsync(meterReading);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<MeterReadingDto>(meterReading);
        return ApiResponse<MeterReadingDto>.Ok(dto, "Meter reading added successfully.");
    }

    public async Task<ApiResponse<IEnumerable<MeterReadingDto>>> GetMeterReadingsByRoomAsync(Guid landlordId, Guid roomId)
    {
        var room = await roomRepository.GetByIdAsync(roomId)
            ?? throw new NotFoundException(nameof(Room), roomId);

        var property = await propertyRepository.GetByIdAsync(room.PropertyId)
            ?? throw new NotFoundException(nameof(Property), room.PropertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to view meter readings for this room.");

        var readings = await meterReadingRepository.FindAsync(m => m.RoomId == roomId);
        var dtos = mapper.Map<IEnumerable<MeterReadingDto>>(readings.OrderByDescending(m => m.Year).ThenByDescending(m => m.Month));
        return ApiResponse<IEnumerable<MeterReadingDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<IEnumerable<MeterReadingDto>>> GetMeterReadingsByPropertyAsync(Guid landlordId, Guid propertyId, int month, int year)
    {
        var property = await propertyRepository.GetByIdAsync(propertyId)
            ?? throw new NotFoundException(nameof(Property), propertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to view meter readings for this property.");

        // We need all rooms in this property to filter meter readings
        var rooms = await roomRepository.FindAsync(r => r.PropertyId == propertyId);
        var roomIds = rooms.Select(r => r.Id).ToList();

        var readings = await meterReadingRepository.FindAsync(m => roomIds.Contains(m.RoomId) && m.Month == month && m.Year == year);
        var dtos = mapper.Map<IEnumerable<MeterReadingDto>>(readings);
        return ApiResponse<IEnumerable<MeterReadingDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<bool>> DeleteMeterReadingAsync(Guid landlordId, Guid id)
    {
        var meterReading = await meterReadingRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(MeterReading), id);

        var room = await roomRepository.GetByIdAsync(meterReading.RoomId)
            ?? throw new NotFoundException(nameof(Room), meterReading.RoomId);

        var property = await propertyRepository.GetByIdAsync(room.PropertyId)
            ?? throw new NotFoundException(nameof(Property), room.PropertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to delete this meter reading.");

        await meterReadingRepository.SoftDeleteAsync(id);
        await unitOfWork.CommitAsync();

        return ApiResponse<bool>.Ok(true, "Meter reading deleted successfully.");
    }
}
