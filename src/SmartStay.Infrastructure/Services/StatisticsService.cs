using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Statistics;
using SmartStay.Application.Interfaces;
using SmartStay.Domain.Enums;
using SmartStay.Infrastructure.Persistence;

namespace SmartStay.Infrastructure.Services;

public class StatisticsService(AppDbContext dbContext) : IStatisticsService
{
    public async Task<ApiResponse<FinanceSummaryDto>> GetFinanceSummaryAsync(Guid landlordId, int? month, int? year)
    {
        var query = from invoice in dbContext.Invoices
                    join room in dbContext.Rooms on invoice.RoomId equals room.Id
                    join property in dbContext.Properties on room.PropertyId equals property.Id
                    where property.LandlordId == landlordId && !invoice.IsDeleted
                    select invoice;

        if (month.HasValue && year.HasValue)
        {
            query = query.Where(i => i.Month == month.Value && i.Year == year.Value);
        }

        var totalRevenue = await query.SumAsync(i => i.TotalAmount);
        var collected = await query.Where(i => i.Status == InvoiceStatus.Paid).SumAsync(i => i.TotalAmount);
        var uncollected = await query.Where(i => i.Status != InvoiceStatus.Paid).SumAsync(i => i.TotalAmount);

        var summary = new FinanceSummaryDto
        {
            TotalRevenue = totalRevenue,
            Collected = collected,
            Uncollected = uncollected
        };

        return ApiResponse<FinanceSummaryDto>.Ok(summary);
    }

    public async Task<ApiResponse<DashboardStatsDto>> GetLandlordDashboardAsync(Guid landlordId, Guid? propertyId)
    {
        var now = DateTime.UtcNow;
        var currentMonth = now.Month;
        var currentYear = now.Year;
        var next30Days = now.AddDays(30);

        // Base query for properties belonging to this landlord
        var propertyQuery = dbContext.Properties.Where(p => p.LandlordId == landlordId && !p.IsDeleted);
        if (propertyId.HasValue)
        {
            propertyQuery = propertyQuery.Where(p => p.Id == propertyId.Value);
        }

        var propertyIds = await propertyQuery.Select(p => p.Id).ToListAsync();

        // 1. Rooms Stats
        var rooms = await dbContext.Rooms
            .Where(r => propertyIds.Contains(r.PropertyId) && !r.IsDeleted)
            .ToListAsync();

        var totalRooms = rooms.Count;
        var emptyRooms = rooms.Count(r => r.Status == RoomStatus.Available || r.Status == RoomStatus.Empty || r.Status == RoomStatus.Maintenance);
        var occupiedRooms = rooms.Count(r => r.Status == RoomStatus.Occupied || r.Status == RoomStatus.Rented);
        var occupancyRate = totalRooms > 0 ? (double)occupiedRooms / totalRooms * 100 : 0;

        // 2. Finance Stats (Current Month)
        var invoices = await dbContext.Invoices
            .Where(i => propertyIds.Contains(dbContext.Rooms.First(r => r.Id == i.RoomId).PropertyId) && !i.IsDeleted) // This is inefficient in SQL, let's optimize
            .ToListAsync();
        
        // Better way for finance:
        var invoiceQuery = from invoice in dbContext.Invoices
                           join room in dbContext.Rooms on invoice.RoomId equals room.Id
                           where propertyIds.Contains(room.PropertyId) && !invoice.IsDeleted
                           select invoice;

        var currentMonthInvoices = await invoiceQuery
            .Where(i => i.Month == currentMonth && i.Year == currentYear)
            .ToListAsync();

        var totalRevenue = currentMonthInvoices.Where(i => i.Status == InvoiceStatus.Paid).Sum(i => i.TotalAmount);
        var uncollected = await invoiceQuery.Where(i => i.Status != InvoiceStatus.Paid).SumAsync(i => i.TotalAmount);

        // 3. Banner Stats
        // Expiring Contracts (< 30 days)
        var expiringContracts = await (from contract in dbContext.Contracts
                                       join room in dbContext.Rooms on contract.RoomId equals room.Id
                                       where propertyIds.Contains(room.PropertyId) && !contract.IsDeleted
                                       && contract.Status == ContractStatus.Active
                                       && contract.EndDate >= now && contract.EndDate <= next30Days
                                       select contract).CountAsync();

        // Rooms with unpaid invoices
        var roomsWithUnpaidInvoices = await (from invoice in dbContext.Invoices
                                             join room in dbContext.Rooms on invoice.RoomId equals room.Id
                                             where propertyIds.Contains(room.PropertyId) && !invoice.IsDeleted
                                             && invoice.Status != InvoiceStatus.Paid
                                             select room.Id).Distinct().CountAsync();

        // Pending Tickets
        var pendingTickets = await (from ticket in dbContext.Tickets
                                    join room in dbContext.Rooms on ticket.RoomId equals room.Id
                                    where propertyIds.Contains(room.PropertyId) && !ticket.IsDeleted
                                    && ticket.Status == TicketStatus.Pending
                                    select ticket).CountAsync();

        var stats = new DashboardStatsDto
        {
            EmptyRoomsCount = emptyRooms,
            TotalRoomsCount = totalRooms,
            OccupiedRoomsCount = occupiedRooms,
            OccupancyRate = Math.Round(occupancyRate, 1),
            TotalRevenueCurrentMonth = totalRevenue,
            UncollectedAmount = uncollected,
            ExpiringContractsCount = expiringContracts,
            UnpaidRoomsCount = roomsWithUnpaidInvoices,
            PendingTicketsCount = pendingTickets
        };

        return ApiResponse<DashboardStatsDto>.Ok(stats);
    }
}
