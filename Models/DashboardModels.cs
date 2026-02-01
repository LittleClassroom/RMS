using System;
using System.Collections.Generic;

namespace RMS.Models
{
    public class DashboardStats
    {
        public int TotalTables { get; set; }
        public int OccupiedTables { get; set; }
        public int OpenOrders { get; set; }
        public int KitchenQueue { get; set; }
        public decimal TodaysSales { get; set; }
        public int OrdersToday { get; set; }
        public int GuestsToday { get; set; }
        public double? AvgSessionMinutesToday { get; set; }
        public int LowStockItems { get; set; }
    }

    public class DashboardActivity
    {
        public DateTime TimestampUtc { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string PerformedBy { get; set; } = string.Empty;
    }

    public class DashboardSnapshot
    {
        public DashboardStats Stats { get; set; } = new DashboardStats();
        public IReadOnlyList<DashboardActivity> Activities { get; set; } = Array.Empty<DashboardActivity>();
    }
}
