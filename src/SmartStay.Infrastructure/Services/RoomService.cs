using System;
using System.Collections.Generic;
using System.Linq;
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
    IRepository<Contract> contractRepository,
    IRepository<User> userRepository,
    IRepository<Invoice> invoiceRepository,
    IRepository<InventoryItem> inventoryItemRepository,
    IRepository<MeterReading> meterReadingRepository,
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

        var activeContracts = await contractRepository.FindAsync(c =>
            c.RoomId == roomId &&
            c.Status == SmartStay.Domain.Enums.ContractStatus.Active);

        if (activeContracts.Any())
            throw new BadRequestException("Phòng này đang có hợp đồng hiệu lực.");

        await roomRepository.SoftDeleteAsync(roomId);
        await unitOfWork.CommitAsync();

        return ApiResponse<bool>.Ok(true, "Room deleted successfully.");
    }

    public async Task<ApiResponse<RoomDto>> GetRoomByIdAsync(Guid roomId)
    {
        var room = await roomRepository.GetByIdAsync(roomId)
            ?? throw new NotFoundException(nameof(Room), roomId);

        var dto = mapper.Map<RoomDto>(room);

        // 1. Fetch Active Contract & Tenant
        var contracts = await contractRepository.FindAsync(c => c.RoomId == roomId && c.Status == SmartStay.Domain.Enums.ContractStatus.Active && !c.IsDeleted);
        var activeContract = System.Linq.Enumerable.FirstOrDefault(contracts.OrderByDescending(c => c.CreatedAt));

        if (activeContract != null)
        {
            dto.Contract = mapper.Map<SmartStay.Application.DTOs.Contract.ContractDto>(activeContract);

            var tenant = await userRepository.GetByIdAsync(activeContract.TenantId);
            if (tenant != null)
            {
                dto.Tenant = new TenantDto
                {
                    Id = tenant.Id,
                    FullName = tenant.FullName,
                    Phone = tenant.Phone,
                    Email = tenant.Email
                };
            }

            var inventoryItems = await inventoryItemRepository.FindAsync(i => i.ContractId == activeContract.Id && !i.IsDeleted);
            dto.InventoryItems = mapper.Map<System.Collections.Generic.List<SmartStay.Application.DTOs.InventoryItem.InventoryItemDto>>(inventoryItems);
        }

        // 2. Fetch Latest Meter Readings (Electric & Water)
        var meterReadings = await meterReadingRepository.FindAsync(m => m.RoomId == roomId && !m.IsDeleted);
        var latestReadings = meterReadings
            .GroupBy(m => m.Type)
            .Select(g => g.OrderByDescending(m => m.Year).ThenByDescending(m => m.Month).First())
            .ToList();
        
        dto.LatestMeterReadings = mapper.Map<List<SmartStay.Application.DTOs.MeterReading.MeterReadingDto>>(latestReadings);

        // 3. Invoices
        var invoices = await invoiceRepository.FindAsync(i => i.RoomId == roomId && !i.IsDeleted);
        dto.Invoices = mapper.Map<System.Collections.Generic.List<SmartStay.Application.DTOs.Invoice.InvoiceDto>>(invoices.OrderByDescending(i => i.Year).ThenByDescending(i => i.Month));

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
