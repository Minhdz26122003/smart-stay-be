using System;

namespace SmartStay.Application.DTOs.Statistics;

public class FinanceSummaryDto
{
    public decimal TotalRevenue { get; set; }
    public decimal Collected { get; set; }
    public decimal Uncollected { get; set; }
}
