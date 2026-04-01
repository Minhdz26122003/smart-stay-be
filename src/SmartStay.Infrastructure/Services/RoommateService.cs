using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Roommate;
using SmartStay.Application.Interfaces;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Exceptions;
using SmartStay.Domain.Interfaces;

namespace SmartStay.Infrastructure.Services;

public class RoommateService(
    IRepository<Roommate> roommateRepository,
    IRepository<Contract> contractRepository,
    IRepository<Room> roomRepository,
    IRepository<Property> propertyRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRoommateService
{
    public async Task<ApiResponse<RoommateDto>> CreateRoommateAsync(Guid currentUserId, CreateRoommateRequest request)
    {
        var contract = await contractRepository.GetByIdAsync(request.ContractId)
            ?? throw new NotFoundException(nameof(Contract), request.ContractId);

        var room = await roomRepository.GetByIdAsync(contract.RoomId)
            ?? throw new NotFoundException(nameof(Room), contract.RoomId);

        var property = await propertyRepository.GetByIdAsync(room.PropertyId)
            ?? throw new NotFoundException(nameof(Property), room.PropertyId);

        bool isTenant = contract.TenantId == currentUserId;
        bool isLandlord = property.LandlordId == currentUserId;

        if (!isTenant && !isLandlord)
            throw new UnauthorizedException("You do not have permission to add roommates to this contract.");

        var roommate = mapper.Map<Roommate>(request);
        // Landlord adds -> auto approved. Tenant adds -> pending approval (false)
        roommate.IsApproved = isLandlord;

        await roommateRepository.AddAsync(roommate);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<RoommateDto>(roommate);
        return ApiResponse<RoommateDto>.Ok(dto, "Roommate added successfully.");
    }

    public async Task<ApiResponse<IEnumerable<RoommateDto>>> GetRoommatesByContractAsync(Guid currentUserId, Guid contractId)
    {
        var contract = await contractRepository.GetByIdAsync(contractId)
            ?? throw new NotFoundException(nameof(Contract), contractId);

        var room = await roomRepository.GetByIdAsync(contract.RoomId)
            ?? throw new NotFoundException(nameof(Room), contract.RoomId);

        var property = await propertyRepository.GetByIdAsync(room.PropertyId)
            ?? throw new NotFoundException(nameof(Property), room.PropertyId);

        bool isTenant = contract.TenantId == currentUserId;
        bool isLandlord = property.LandlordId == currentUserId;

        if (!isTenant && !isLandlord)
            throw new UnauthorizedException("You do not have permission to view roommates for this contract.");

        var roommates = await roommateRepository.FindAsync(r => r.ContractId == contractId);
        var dtos = mapper.Map<IEnumerable<RoommateDto>>(roommates.OrderBy(r => r.CreatedAt));
        return ApiResponse<IEnumerable<RoommateDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<RoommateDto>> ApproveRoommateAsync(Guid landlordId, Guid roommateId)
    {
        var roommate = await roommateRepository.GetByIdAsync(roommateId)
            ?? throw new NotFoundException(nameof(Roommate), roommateId);

        var contract = await contractRepository.GetByIdAsync(roommate.ContractId)
            ?? throw new NotFoundException(nameof(Contract), roommate.ContractId);

        var room = await roomRepository.GetByIdAsync(contract.RoomId)
            ?? throw new NotFoundException(nameof(Room), contract.RoomId);

        var property = await propertyRepository.GetByIdAsync(room.PropertyId)
            ?? throw new NotFoundException(nameof(Property), room.PropertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("Only the landlord can approve this roommate.");

        roommate.IsApproved = true;
        roommateRepository.Update(roommate);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<RoommateDto>(roommate);
        return ApiResponse<RoommateDto>.Ok(dto, "Roommate approved successfully.");
    }

    public async Task<ApiResponse<bool>> DeleteRoommateAsync(Guid currentUserId, Guid roommateId)
    {
        var roommate = await roommateRepository.GetByIdAsync(roommateId)
            ?? throw new NotFoundException(nameof(Roommate), roommateId);

        var contract = await contractRepository.GetByIdAsync(roommate.ContractId)
            ?? throw new NotFoundException(nameof(Contract), roommate.ContractId);

        var room = await roomRepository.GetByIdAsync(contract.RoomId)
            ?? throw new NotFoundException(nameof(Room), contract.RoomId);

        var property = await propertyRepository.GetByIdAsync(room.PropertyId)
            ?? throw new NotFoundException(nameof(Property), room.PropertyId);

        bool isTenant = contract.TenantId == currentUserId;
        bool isLandlord = property.LandlordId == currentUserId;

        if (!isTenant && !isLandlord)
            throw new UnauthorizedException("You do not have permission to remove this roommate.");

        await roommateRepository.SoftDeleteAsync(roommateId);
        await unitOfWork.CommitAsync();

        return ApiResponse<bool>.Ok(true, "Roommate removed successfully.");
    }
}
