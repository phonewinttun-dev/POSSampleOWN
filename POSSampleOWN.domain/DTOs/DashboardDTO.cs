using System;
using System.Collections.Generic;

namespace POSSampleOWN.domain.DTOs;

// ─── [1] Sales Overview ───
public class SalesOverviewDTO
{
    public decimal TotalRevenue { get; set; }
    public int TotalSales { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

// ─── [2] Sales Per Period ───
public class SalesPerPeriodDTO
{
    public string Period { get; set; } = null!;
    public List<SalesPeriodGroupDTO> Data { get; set; } = new();
}

public class SalesPeriodGroupDTO
{
    public string Label { get; set; } = null!;
    public decimal TotalRevenue { get; set; }
    public int TotalSales { get; set; }
}

// ─── [3] Sales Report ───
public class SalesReportDTO
{
    public string ReportRange { get; set; } = null!;
    public decimal TotalRevenue { get; set; }
    public int TotalSales { get; set; }
    public List<SalesReportGroupDTO> MonthlySummary { get; set; } = new();
}

public class SalesReportGroupDTO
{
    public string Month { get; set; } = null!;
    public decimal Revenue { get; set; }
    public int Sales { get; set; }
}

// ─── [4] Top Products ───
public class TopProductDTO
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int TotalQuantitySold { get; set; }
    public decimal TotalRevenue { get; set; }
}
