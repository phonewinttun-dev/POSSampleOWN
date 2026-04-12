using Microsoft.AspNetCore.Mvc;
using POSSampleOWN.domain.Features.Dashboard;
using POSSampleOWN.Responses;
using System;

namespace POSSampleOWN.Controllers;

[Route("api/dashboard")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service)
    {
        _service = service;
    }

    // GET: api/dashboard/overview?startDate=2026-01-01&endDate=2026-04-11
    [HttpGet("overview")]
    public IActionResult GetSalesOverview([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var result = _service.GetSalesOverview(startDate, endDate);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    // GET: api/dashboard/sales-per-period?period=day
    [HttpGet("sales-per-period")]
    public IActionResult GetSalesPerPeriod([FromQuery] string period = "day")
    {
        var result = _service.GetSalesPerPeriod(period);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    // GET: api/dashboard/report?range=1month
    [HttpGet("report")]
    public IActionResult GetSalesReport([FromQuery] string range = "1month")
    {
        var result = _service.GetSalesReport(range);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    // GET: api/dashboard/top-products?top=10
    [HttpGet("top-products")]
    public IActionResult GetTopProducts([FromQuery] int top = 10)
    {
        var result = _service.GetTopProducts(top);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }
}
