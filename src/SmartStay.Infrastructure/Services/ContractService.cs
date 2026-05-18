using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Contract;
using SmartStay.Application.Interfaces;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Enums;
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

    private static void ValidateContractDates(DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate)
            throw new BadRequestException("Contract end date must be after start date.");
    }

    private static void ValidateDepositAmount(decimal depositAmount)
    {
        if (depositAmount < 0)
            throw new BadRequestException("Deposit amount cannot be negative.");
    }

    private async Task EnsureNoActiveContractForRoomAsync(Guid roomId, Guid? excludeContractId = null)
    {
        var contracts = await contractRepository.FindAsync(c => c.RoomId == roomId);
        var hasActiveContract = contracts.Any(c =>
            c.Status == ContractStatus.Active &&
            c.Id != excludeContractId);

        if (hasActiveContract)
            throw new BadRequestException("The room already has an active contract.");
    }

    public async Task<ApiResponse<ContractDto>> CreateContractAsync(Guid landlordId, CreateContractRequest request)
    {
        ValidateContractDates(request.StartDate, request.EndDate);
        ValidateDepositAmount(request.DepositAmount);
        await VerifyLandlordOwnsRoomAsync(landlordId, request.RoomId);
        await EnsureNoActiveContractForRoomAsync(request.RoomId);

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

    public async Task<ApiResponse<IEnumerable<ContractDto>>> GetContractsByPropertyAsync(Guid propertyId)
    {
        var rooms = await roomRepository.FindAsync(r => r.PropertyId == propertyId);
        var roomIds = rooms.Select(r => r.Id).ToList();

        var contracts = await contractRepository.FindAsync(c => roomIds.Contains(c.RoomId));
        var dtos = mapper.Map<IEnumerable<ContractDto>>(contracts);
        return ApiResponse<IEnumerable<ContractDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<IEnumerable<ContractDto>>> GetContractsByLandlordAsync(Guid landlordId)
    {
        var properties = await propertyRepository.FindAsync(p => p.LandlordId == landlordId);
        var propertyIds = properties.Select(p => p.Id).ToList();

        var rooms = await roomRepository.FindAsync(r => propertyIds.Contains(r.PropertyId));
        var roomIds = rooms.Select(r => r.Id).ToList();

        var contracts = await contractRepository.FindAsync(c => roomIds.Contains(c.RoomId));
        var dtos = mapper.Map<IEnumerable<ContractDto>>(contracts);
        return ApiResponse<IEnumerable<ContractDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<ContractDto>> UpdateContractAsync(Guid landlordId, Guid contractId, UpdateContractRequest request)
    {
        var contract = await contractRepository.GetByIdAsync(contractId)
            ?? throw new NotFoundException(nameof(Contract), contractId);

        await VerifyLandlordOwnsRoomAsync(landlordId, contract.RoomId);

        if (request.Status == ContractStatus.Active)
            await EnsureNoActiveContractForRoomAsync(contract.RoomId, contract.Id);

        mapper.Map(request, contract);
        contractRepository.Update(contract);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<ContractDto>(contract);
        return ApiResponse<ContractDto>.Ok(dto, "Contract updated successfully.");
    }
}
