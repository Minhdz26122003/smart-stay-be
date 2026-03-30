using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Contract;
using SmartStay.Application.Interfaces;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Exceptions;
using SmartStay.Domain.Interfaces;

namespace SmartStay.Infrastructure.Services;

public class ContractService(
    IRepository<Contract> contractRepository,
    IRepository<Room> roomRepository,
    IRepository<Property> propertyRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IContractService
{
    private async Task VerifyLandlordOwnsRoomAsync(Guid landlordId, Guid roomId)
    {
        var room = await roomRepository.GetByIdAsync(roomId)
            ?? throw new NotFoundException(nameof(Room), roomId);

        var property = await propertyRepository.GetByIdAsync(room.PropertyId);
        if (property == null || property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to manage contracts for this room.");
    }

    public async Task<ApiResponse<ContractDto>> CreateContractAsync(Guid landlordId, CreateContractRequest request)
    {
        await VerifyLandlordOwnsRoomAsync(landlordId, request.RoomId);

        var contract = mapper.Map<Contract>(request);
        await contractRepository.AddAsync(contract);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<ContractDto>(contract);
        return ApiResponse<ContractDto>.Ok(dto, "Contract created successfully.");
    }

    public async Task<ApiResponse<bool>> DeleteContractAsync(Guid landlordId, Guid contractId)
    {
        var contract = await contractRepository.GetByIdAsync(contractId)
            ?? throw new NotFoundException(nameof(Contract), contractId);

        await VerifyLandlordOwnsRoomAsync(landlordId, contract.RoomId);

        await contractRepository.SoftDeleteAsync(contractId);
        await unitOfWork.CommitAsync();

        return ApiResponse<bool>.Ok(true, "Contract deleted successfully.");
    }

    public async Task<ApiResponse<ContractDto>> GetContractByIdAsync(Guid contractId)
    {
        var contract = await contractRepository.GetByIdAsync(contractId)
            ?? throw new NotFoundException(nameof(Contract), contractId);

        var dto = mapper.Map<ContractDto>(contract);
        return ApiResponse<ContractDto>.Ok(dto);
    }

    public async Task<ApiResponse<IEnumerable<ContractDto>>> GetContractsByRoomAsync(Guid roomId)
    {
        var contracts = await contractRepository.FindAsync(c => c.RoomId == roomId);
        var dtos = mapper.Map<IEnumerable<ContractDto>>(contracts);
        return ApiResponse<IEnumerable<ContractDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<IEnumerable<ContractDto>>> GetContractsByTenantAsync(Guid tenantId)
    {
        var contracts = await contractRepository.FindAsync(c => c.TenantId == tenantId);
        var dtos = mapper.Map<IEnumerable<ContractDto>>(contracts);
        return ApiResponse<IEnumerable<ContractDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<ContractDto>> UpdateContractAsync(Guid landlordId, Guid contractId, UpdateContractRequest request)
    {
        var contract = await contractRepository.GetByIdAsync(contractId)
            ?? throw new NotFoundException(nameof(Contract), contractId);

        await VerifyLandlordOwnsRoomAsync(landlordId, contract.RoomId);

        mapper.Map(request, contract);
        contractRepository.Update(contract);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<ContractDto>(contract);
        return ApiResponse<ContractDto>.Ok(dto, "Contract updated successfully.");
    }
}
