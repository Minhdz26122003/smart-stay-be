using System;

namespace SmartStay.Application.DTOs.Statistics;

public class DashboardStatsDto
{
    // Card 1: Phòng trống
    public int EmptyRoomsCount { get; set; }
    public int TotalRoomsCount { get; set; }

    // Card 2: Đang thuê
    public int OccupiedRoomsCount { get; set; }
    public double OccupancyRate { get; set; }

    // Card 3: Doanh thu (Monthly) 
    public decimal TotalRevenueCurrentMonth { get; set; }
    
    // Card 4: Chưa thu
    public decimal UncollectedAmount { get; set; }

    // Banners
    public int ExpiringContractsCount { get; set; }
    public int UnpaidRoomsCount { get; set; }
    public int PendingTicketsCount { get; set; }
}
