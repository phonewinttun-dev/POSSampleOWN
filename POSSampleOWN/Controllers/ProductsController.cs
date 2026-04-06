using Microsoft.AspNetCore.Mvc;
using POSSampleOWN.DTOs;
using POSSampleOWN.Models;
using POSSampleOWN.Responses;
using POSSampleOWN.Services;
using System.Threading.Tasks;

namespace POSSampleOWN.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products/
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();

            return Ok(ApiResponse<List<ProductDTO>>.Success(products));
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);

                if (product is null)
                {
                    return NotFound(ApiResponse<ProductDTO>.Fail("Product not found."));
                }

                return Ok(ApiResponse<ProductDTO>.Success(product));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ProductDTO>.Fail($"An error occurred while retrieving the product: {ex.Message}"));
            }

        }

        // GET: api/products/availableProducts
        [HttpGet("availableProducts")]
        public async Task<IActionResult> GetAvailable()
        {
            var availableProducts = await _productService.GetAvailableAsync();

            return Ok(availableProducts);
        }

        // POST: api/products/
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDTO createRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "Invalid product data."
                });

            try
            {
                var createdProduct = await _productService.CreateAsync(createRequest);

                if (createdProduct is null)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        IsSuccess = false,
                        Message = "Failed to create product."
                    });
                }

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdProduct.Id },
                    ApiResponse<ProductDTO>.Success(createdProduct, "Product created successfully."));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while creating the product: {ex.Message}"
                });
            }
        }

        // PATCH: api/products/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDTO updateRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "Invalid product data."
                });

            try
            {
                var updatedProduct = await _productService.UpdateAsync(id, updateRequest);

                if (updatedProduct is null)
                {
                    return NotFound(ApiResponse<ProductDTO>.Fail("Product not found."));
                }

                return Ok(ApiResponse<ProductDTO>.Success(updatedProduct, "Updated successfully."));
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message));
            }
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "Invalid product ID."
                });
            try
            {
                var result = await _productService.DeleteAsync(id);

                return Ok(ApiResponse<object>.Success(result, "Deleted successfully."));
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message));
            }
        }

        // GET : api/products/search?term=searchTerm
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "Search term cannot be empty.",
                });
            }
            try
            {
                var results = await _productService.GetProductsByTermAsync(term);
                return Ok(ApiResponse<List<ProductDTO>>.Success(results, "Successfully Retrieved"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ProductDTO>.Fail($"An error occurred while retrieving: {ex.Message}"));
            }

        }
    }

}