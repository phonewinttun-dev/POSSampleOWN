using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using POSSampleOWN.DTOs;
using POSSampleOWN.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSampleOWN.domain.Features.Search
{
    [ApiController]
    [Route("api/search")]
    [Authorize(Roles = "Admin,Staff")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _service;

        public SearchController(ISearchService service)
        {
            _service = service;
        }


        // GET : api/search
        [HttpGet]
        public async Task<IActionResult> SearchProducts([FromQuery] SearchRequestDTO searchRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "Invalid search parameters."
                });
            }
            try
            {
                var products = await _service.SearchProductsAsync(searchRequest);
                return Ok(new ApiResponse<object>
                {
                    IsSuccess = true,
                    Message = "Search completed successfully.",
                    Data = products
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while searching for products: {ex.Message}"
                });
            }



        }
    }
}
