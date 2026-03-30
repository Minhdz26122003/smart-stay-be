using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Room;
using SmartStay.Application.Interfaces;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Exceptions;
using SmartStay.Domain.Interfaces;

namespace SmartStay.Infrastructure.Services;

public class RoomService(
    IRepository<Room> roomRepository,
    IRepository<Property> propertyRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRoomService
{
    public async Task<ApiResponse<RoomDto>> CreateRoomAsync(Guid landlordId, CreateRoomRequest request)
    {
        var property = await propertyRepository.GetByIdAsync(request.PropertyId)
            ?? throw new NotFoundException(nameof(Property), request.PropertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to add a room to this property.");

        var room = mapper.Map<Room>(request);
        await roomRepository.AddAsync(room);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<RoomDto>(room);
        return ApiResponse<RoomDto>.Ok(dto, "Room created successfully.");
    }

    public async Task<ApiResponse<bool>> DeleteRoomAsync(Guid landlordId, Guid roomId)
    {
        var room = await roomRepository.GetByIdAsync(roomId)
            ?? throw new NotFoundException(nameof(Room), roomId);

        var property = await propertyRepository.GetByIdAsync(room.PropertyId);
        if (property == null || property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to delete this room.");

        await roomRepository.SoftDeleteAsync(roomId);
        await unitOfWork.CommitAsync();

        return ApiResponse<bool>.Ok(true, "Room deleted successfully.");
    }

    public async Task<ApiResponse<RoomDto>> GetRoomByIdAsync(Guid roomId)
    {
        var room = await roomRepository.GetByIdAsync(roomId)
            ?? throw new NotFoundException(nameof(Room), roomId);

        var dto = mapper.Map<RoomDto>(room);
        return ApiResponse<RoomDto>.Ok(dto);
    }

    public async Task<ApiResponse<IEnumerable<RoomDto>>> GetRoomsByPropertyAsync(Guid propertyId)
    {
        var rooms = await roomRepository.FindAsync(r => r.PropertyId == propertyId);
        var dtos = mapper.Map<IEnumerable<RoomDto>>(rooms);
        return ApiResponse<IEnumerable<RoomDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<RoomDto>> UpdateRoomAsync(Guid landlordId, Guid roomId, UpdateRoomRequest request)
    {
        var room = await roomRepository.GetByIdAsync(roomId)
            ?? throw new NotFoundException(nameof(Room), roomId);

        var property = await propertyRepository.GetByIdAsync(room.PropertyId);
        if (property == null || property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to update this room.");

        mapper.Map(request, room);
        roomRepository.Update(room);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<RoomDto>(room);
        return ApiResponse<RoomDto>.Ok(dto, "Room updated successfully.");
    }
}
