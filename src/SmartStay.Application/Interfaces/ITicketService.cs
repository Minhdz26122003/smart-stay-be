using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Ticket;

namespace SmartStay.Application.Interfaces;

public interface ITicketService
{
    Task<ApiResponse<TicketDto>> CreateTicketAsync(Guid tenantId, CreateTicketRequest request);
    Task<ApiResponse<IEnumerable<TicketDto>>> GetTicketsByTenantAsync(Guid tenantId);
    Task<ApiResponse<IEnumerable<TicketDto>>> GetTicketsByLandlordAsync(Guid landlordId);
    Task<ApiResponse<TicketDto>> UpdateTicketStatusAsync(Guid landlordId, Guid ticketId, UpdateTicketStatusRequest request);
}
