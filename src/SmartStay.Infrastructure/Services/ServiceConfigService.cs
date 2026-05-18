using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.ServiceConfig;
using SmartStay.Application.Interfaces;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Exceptions;
using SmartStay.Domain.Interfaces;

namespace SmartStay.Infrastructure.Services;

public class ServiceConfigService(
    IRepository<ServiceConfig> serviceConfigRepository,
    IRepository<Property> propertyRepository,
    IRepository<Room> roomRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IServiceConfigService
{
    public async Task<ApiResponse<ServiceConfigDto>> CreateServiceConfigAsync(Guid landlordId, CreateServiceConfigRequest request)
    {
        var property = await propertyRepository.GetByIdAsync(request.PropertyId)
            ?? throw new NotFoundException(nameof(Property), request.PropertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to modify configurations for this property.");

        if (request.UnitPrice < 0)
            throw new BadRequestException("Unit price cannot be negative.");

        if (string.IsNullOrWhiteSpace(request.Type))
            throw new BadRequestException("Service type is required.");

        if (request.RoomId.HasValue)
        {
            var room = await roomRepository.GetByIdAsync(request.RoomId.Value)
                ?? throw new NotFoundException(nameof(Room), request.RoomId.Value);

            if (room.PropertyId != request.PropertyId)
                throw new BadRequestException("The selected room does not belong to the property.");
        }

        var serviceConfig = mapper.Map<ServiceConfig>(request);
        await serviceConfigRepository.AddAsync(serviceConfig);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<ServiceConfigDto>(serviceConfig);
        return ApiResponse<ServiceConfigDto>.Ok(dto, "Service configuration created successfully.");
    }

    public async Task<ApiResponse<ServiceConfigDto>> UpdateServiceConfigAsync(Guid landlordId, Guid configId, UpdateServiceConfigRequest request)
    {
        var config = await serviceConfigRepository.GetByIdAsync(configId)
            ?? throw new NotFoundException(nameof(ServiceConfig), configId);

        var property = await propertyRepository.GetByIdAsync(config.PropertyId)
            ?? throw new NotFoundException(nameof(Property), config.PropertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to modify configurations for this property.");

        if (request.UnitPrice < 0)
            throw new BadRequestException("Unit price cannot be negative.");

        mapper.Map(request, config);
        serviceConfigRepository.Update(config);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<ServiceConfigDto>(config);
        return ApiResponse<ServiceConfigDto>.Ok(dto, "Service configuration updated successfully.");
    }

    public async Task<ApiResponse<IEnumerable<ServiceConfigDto>>> GetServiceConfigsByPropertyIdAsync(Guid propertyId)
    {
        var configs = await serviceConfigRepository.FindAsync(c => c.PropertyId == propertyId);
        var dtos = mapper.Map<IEnumerable<ServiceConfigDto>>(configs);
        return ApiResponse<IEnumerable<ServiceConfigDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<bool>> DeleteServiceConfigAsync(Guid landlordId, Guid configId)
    {
        var config = await serviceConfigRepository.GetByIdAsync(configId)
            ?? throw new NotFoundException(nameof(ServiceConfig), configId);

        var property = await propertyRepository.GetByIdAsync(config.PropertyId)
            ?? throw new NotFoundException(nameof(Property), config.PropertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to delete configurations for this property.");

        await serviceConfigRepository.SoftDeleteAsync(configId);
        await unitOfWork.CommitAsync();

        return ApiResponse<bool>.Ok(true, "Service configuration deleted successfully.");
    }
}
