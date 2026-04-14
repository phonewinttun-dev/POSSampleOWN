using Microsoft.AspNetCore.Mvc;
using POSSampleOWN.DTOs;
using POSSampleOWN.Responses;
using System.Threading.Tasks;
using POSSampleOWN.domain.Features.ProductsCatalog;

namespace POSSampleOWN.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly IProductCatalogService _service;

        public CategoriesController(IProductCatalogService service)
        {
            _service = service;
        }

        // GET: api/categories/
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllCategoriesAsync();
            return Ok(result);
        }

        // GET: api/categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetCategoryByIdAsync(id);

            if (!result.IsSuccess) return NotFound(result);

            return Ok(result);
        }

        // POST: api/categories/
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateCategoryAsync(request);

            if (!result.IsSuccess) return BadRequest(result);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Data!.Id },
                result);
        }

        // PATCH: api/categories/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateCategoryAsync(id, request);

            if (!result.IsSuccess)
                return result.Message.Contains("not found") ? NotFound(result) : BadRequest(result);
            
            return Ok(result);
        }

        // DELETE: api/categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {      
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.DeleteCategoryAsync(id);

            if (!result.IsSuccess)
                return result.Message.Contains("not found") ? NotFound(result) : BadRequest(result);
            
            return Ok(result);
        }

        // GET: api/categories/search?term=searchTerm
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest(ApiResponse<object>.Fail("Search term cannot be empty."));

            var result = await _service.GetCategoriesByTermAsync(term);

            if (!result.IsSuccess)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }
    }
}
