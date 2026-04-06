using Microsoft.AspNetCore.Mvc;
using POSSampleOWN.domain.Features.ProductsCatalog;
using POSSampleOWN.DTOs;
using POSSampleOWN.Models;
using POSSampleOWN.Responses;
using System.Threading.Tasks;

namespace POSSampleOWN.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductCatalogService _service;

        public ProductsController(IProductCatalogService service)
        {
            _service = service;
        }

        // GET: api/products/
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAllProductsAsync();

            return Ok(ApiResponse<List<ProductDTO>>.Success(products));
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _service.GetProductByIdAsync(id);

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
            var availableProducts = await _service.GetAvailableProductsAsync();

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
                var createdProduct = await _service.CreateProductAsync(createRequest);

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

        // POST: api/products/bulk
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkCreate([FromBody] List<CreateProductDTO> bulkRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "Invalid data"
                });
            }

            try
            {
                var bulkCreatedProducts = await _service.BulkCreateProductsAsync(bulkRequest);

                if (bulkCreatedProducts is null)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        IsSuccess = false,
                        Message = "Failed to create products."
                    });
                }

                return Ok(new ApiResponse<List<ProductDTO>>
                {
                    IsSuccess = true,
                    Message = $"{bulkCreatedProducts.Count} books created successfully.",
                    Data = bulkCreatedProducts
                });
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
                var updatedProduct = await _service.UpdateProductAsync(id, updateRequest);

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
                var result = await _service.DeleteProductAsync(id);

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
                var results = await _service.GetProductsByTermAsync(term);
                return Ok(ApiResponse<List<ProductDTO>>.Success(results, "Successfully Retrieved"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ProductDTO>.Fail($"An error occurred while retrieving: {ex.Message}"));
            }

        }


    }

}