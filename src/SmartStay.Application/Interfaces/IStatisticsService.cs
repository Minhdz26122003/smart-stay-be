using System;
using System.Threading.Tasks;
using SmartStay.Application.Common.Models;
using SmartStay.Application.DTOs.Statistics;

namespace SmartStay.Application.Interfaces;

public interface IStatisticsService
{
    Task<ApiResponse<FinanceSummaryDto>> GetFinanceSummaryAsync(Guid landlordId, int? month, int? year);
}
