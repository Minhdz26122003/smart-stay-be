using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Ticket;
using SmartStay.Application.Interfaces;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Exceptions;
using SmartStay.Domain.Interfaces;

namespace SmartStay.Infrastructure.Services;

public class TicketService(
    IRepository<Ticket> ticketRepository,
    IRepository<Room> roomRepository,
    IRepository<Property> propertyRepository,
    IRepository<User> userRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : ITicketService
{
    public async Task<ApiResponse<TicketDto>> CreateTicketAsync(Guid tenantId, CreateTicketRequest request)
    {
        var ticket = mapper.Map<Ticket>(request);
        ticket.TenantId = tenantId;
        ticket.Status = SmartStay.Domain.Enums.TicketStatus.Pending;

        await ticketRepository.AddAsync(ticket);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<TicketDto>(ticket);
        return ApiResponse<TicketDto>.Ok(dto, "Ticket created successfully.");
    }

    public async Task<ApiResponse<IEnumerable<TicketDto>>> GetTicketsByTenantAsync(Guid tenantId)
    {
        var tickets = await ticketRepository.FindAsync(t => t.TenantId == tenantId);
        var dtos = mapper.Map<IEnumerable<TicketDto>>(tickets.OrderByDescending(t => t.CreatedAt));
        return ApiResponse<IEnumerable<TicketDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<IEnumerable<TicketDto>>> GetTicketsByLandlordAsync(Guid landlordId)
    {
        // Gộp tất cả các phòng của landlord này
        var properties = await propertyRepository.FindAsync(p => p.LandlordId == landlordId);
        var propertyIds = properties.Select(p => p.Id).ToList();

        var rooms = await roomRepository.FindAsync(r => propertyIds.Contains(r.PropertyId));
        var roomIds = rooms.Select(r => r.Id).ToList();

        var tickets = await ticketRepository.FindAsync(t => roomIds.Contains(t.RoomId));
        
        // Fetch tenants info
        var tenantIds = tickets.Select(t => t.TenantId).Distinct().ToList();
        var users = await userRepository.FindAsync(u => tenantIds.Contains(u.Id));

        var dtos = mapper.Map<IEnumerable<TicketDto>>(tickets.OrderByDescending(t => t.CreatedAt)).ToList();
        
        foreach (var dto in dtos)
        {
            dto.RoomName = rooms.FirstOrDefault(r => r.Id == dto.RoomId)?.Name;
            dto.TenantName = users.FirstOrDefault(u => u.Id == dto.TenantId)?.FullName;
        }

        return ApiResponse<IEnumerable<TicketDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<TicketDto>> UpdateTicketStatusAsync(Guid landlordId, Guid ticketId, UpdateTicketStatusRequest request)
    {
        var ticket = await ticketRepository.GetByIdAsync(ticketId)
            ?? throw new NotFoundException(nameof(Ticket), ticketId);

        var room = await roomRepository.GetByIdAsync(ticket.RoomId)
            ?? throw new NotFoundException(nameof(Room), ticket.RoomId);

        var property = await propertyRepository.GetByIdAsync(room.PropertyId)
            ?? throw new NotFoundException(nameof(Property), room.PropertyId);

        if (property.LandlordId != landlordId)
            throw new UnauthorizedException("You do not have permission to update this ticket.");

        ticket.Status = request.Status;
        ticketRepository.Update(ticket);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<TicketDto>(ticket);
        return ApiResponse<TicketDto>.Ok(dto, "Ticket status updated successfully.");
    }
}
