using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using POSSampleOWN.domain.Features.Sale;
using POSSampleOWN.domain.DTOs;
using POSSampleOWN.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;


namespace POSSampleOWN.Controllers
{
    [Route("api/sales")]
    [ApiController]
    [Authorize(Roles = "Admin,Staff")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _service;

        public SalesController(ISaleService service)
        {
            _service = service;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        // GET: api/sales
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _service.GetAllSales();
            return Ok(result);
        }

        // GET: api/sales/{id}
        [HttpGet("{id}")]
        public IActionResult GetByVoucherCode(string voucherCode)
        {
            var result = _service.GetSaleByVouncherCode(voucherCode);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        // POST: api/sales
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSaleDTO createRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Invalid sale data."));

            var result = await _service.CreateSaleAsync(createRequest, GetCurrentUserId());

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(
                nameof(GetByVoucherCode),
                new { id = result.Data!.Id },
                result);
        }
    }
}
