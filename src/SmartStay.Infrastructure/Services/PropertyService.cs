using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Property;
using SmartStay.Application.Interfaces;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Exceptions;
using SmartStay.Domain.Interfaces;

namespace SmartStay.Infrastructure.Services;

public class PropertyService(
    IRepository<Property> propertyRepository,
    IRepository<Room> roomRepository,
    IRepository<Contract> contractRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IPropertyService
{
    public async Task<ApiResponse<PropertyDto>> CreatePropertyAsync(Guid landlordId, CreatePropertyRequest request)
    {
        var property = mapper.Map<Property>(request);
        property.LandlordId = landlordId;

        await propertyRepository.AddAsync(property);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<PropertyDto>(property);
        return ApiResponse<PropertyDto>.Ok(dto, "Property created successfully.");
    }

    public async Task<ApiResponse<bool>> DeletePropertyAsync(Guid landlordId, Guid propertyId)
    {
        var property = await propertyRepository.GetByIdAsync(propertyId)
            ?? throw new NotFoundException(nameof(Property), propertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to delete this property.");

        var rooms = await roomRepository.FindAsync(r => r.PropertyId == propertyId);
        var roomIds = rooms.Select(r => r.Id).ToList();

        if (roomIds.Count > 0)
        {
            var activeContracts = await contractRepository.FindAsync(c =>
                roomIds.Contains(c.RoomId) &&
                c.Status == SmartStay.Domain.Enums.ContractStatus.Active);

            if (activeContracts.Any())
                throw new BadRequestException("Khu trọ này có phòng đang có hợp đồng hiệu lực nên không thể xóa.");
        }

        await propertyRepository.SoftDeleteAsync(propertyId);
        await unitOfWork.CommitAsync();

        return ApiResponse<bool>.Ok(true, "Property deleted successfully.");
    }

    public async Task<ApiResponse<IEnumerable<PropertyDto>>> GetPropertiesByLandlordAsync(Guid landlordId)
    {
        var properties = await propertyRepository.FindAsync(p => p.LandlordId == landlordId);
        var dtos = mapper.Map<IEnumerable<PropertyDto>>(properties);
        return ApiResponse<IEnumerable<PropertyDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<PropertyDto>> GetPropertyByIdAsync(Guid propertyId)
    {
        var property = await propertyRepository.GetByIdAsync(propertyId)
            ?? throw new NotFoundException(nameof(Property), propertyId);

        var dto = mapper.Map<PropertyDto>(property);
        return ApiResponse<PropertyDto>.Ok(dto);
    }

    public async Task<ApiResponse<PropertyDto>> UpdatePropertyAsync(Guid landlordId, Guid propertyId, UpdatePropertyRequest request)
    {
        var property = await propertyRepository.GetByIdAsync(propertyId)
            ?? throw new NotFoundException(nameof(Property), propertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to update this property.");

        mapper.Map(request, property);
        propertyRepository.Update(property);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<PropertyDto>(property);
        return ApiResponse<PropertyDto>.Ok(dto, "Property updated successfully.");
    }
}
