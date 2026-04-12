using POSSampleOWN.domain.DTOs;
using POSSampleOWN.Responses;
using System;
using System.Collections.Generic;

namespace POSSampleOWN.domain.Features.Dashboard;

public interface IDashboardService
{
    ApiResponse<SalesOverviewDTO> GetSalesOverview(DateTime startDate, DateTime endDate);
    ApiResponse<SalesPerPeriodDTO> GetSalesPerPeriod(string period);
    ApiResponse<SalesReportDTO> GetSalesReport(string range);
    ApiResponse<List<TopProductDTO>> GetTopProducts(int top = 10);
}
