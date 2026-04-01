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
}
