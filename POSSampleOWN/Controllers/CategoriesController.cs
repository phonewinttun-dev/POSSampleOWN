using Microsoft.AspNetCore.Mvc;
using POSSampleOWN.DTOs;
using POSSampleOWN.Responses;
using POSSampleOWN.Services;
using System.Threading.Tasks;

namespace POSSampleOWN.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/categories/
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();

            return Ok(categories);
        }

        // GET: api/categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(ApiResponse<CategoryDTO>.Fail("Category not found."));
            }

            return Ok(ApiResponse<CategoryDTO>.Success(category));
        }

        // POST: api/categories/
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "Invalid category data."
                });
            try
            {
                var createdCategory = await _categoryService.CreateAsync(request);

                if (createdCategory is null)
                {
                    return BadRequest(ApiResponse<object>.Fail("Failed to create category."));
                }

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdCategory.Id },
                    ApiResponse<CategoryDTO>.Success(createdCategory, "Category created successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"An error occurred while creating the category: {ex.Message}"));
            }
        }

        // PATCH: api/categories/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "Invalid category data."
                });
            try
            {
                var updatedCategory = await _categoryService.UpdateAsync(id, request);

                if (updatedCategory is null)
                {
                    return NotFound(ApiResponse<object>.Fail("Category not found."));
                }

                return Ok(ApiResponse<CategoryDTO>.Success(updatedCategory, "Category updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"An error occurred while updating the category: {ex.Message}"));
            }
        }

        // DELETE: api/categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {      
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "Invalid product ID."
                });
            try
            {
                var result = await _categoryService.DeleteAsync(id);

                return Ok(ApiResponse<object>.Success(result, "Deleted successfully."));
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message));
            }
        }
    }
}
