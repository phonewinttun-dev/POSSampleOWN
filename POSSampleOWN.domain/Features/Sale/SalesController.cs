using Microsoft.AspNetCore.Mvc;
using POSSampleOWN.domain.Features.Sale;
using POSSampleOWN.domain.DTOs;
using POSSampleOWN.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSSampleOWN.Controllers
{
    [Route("api/sales")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _service;

        public SalesController(ISaleService service)
        {
            _service = service;
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
        public IActionResult GetById(long id)
        {
            var result = _service.GetSaleById(id);
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

            var result = await _service.CreateSaleAsync(createRequest);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Data!.Id },
                result);
        }
    }
}
