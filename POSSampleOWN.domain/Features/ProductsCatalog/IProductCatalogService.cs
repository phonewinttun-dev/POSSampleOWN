using POSSampleOWN.DTOs;
using POSSampleOWN.Responses;

namespace POSSampleOWN.domain.Features.ProductsCatalog
{
    public interface IProductCatalogService
    {
        Task<ApiResponse<List<ProductDTO>>> GetAllProductsAsync();
        Task<ApiResponse<ProductDTO>> GetProductByIdAsync(int id);
        Task<ApiResponse<List<ProductDTO>>> GetAvailableProductsAsync();
        Task<ApiResponse<ProductDTO>> CreateProductAsync(CreateProductDTO request);

        Task<ApiResponse<List<ProductDTO>>> BulkCreateProductsAsync(List<CreateProductDTO> request);
        Task<ApiResponse<ProductDTO>> UpdateProductAsync(int id, UpdateProductDTO request);
        Task<ApiResponse<bool>> DeleteProductAsync(int id);
        Task<ApiResponse<List<ProductDTO>>> GetProductsByTermAsync(string term);
        Task<ApiResponse<List<CategoryDTO>>> GetAllCategoriesAsync();
        Task<ApiResponse<CategoryDTO>> GetCategoryByIdAsync(int id);
        Task<ApiResponse<CategoryDTO>> CreateCategoryAsync(CreateCategoryDTO request);
        Task<ApiResponse<CategoryDTO>> UpdateCategoryAsync(int id, UpdateCategoryDTO request);
        Task<ApiResponse<List<CategoryDTO>>> GetCategoriesByTermAsync(string term);
        Task<ApiResponse<bool>> DeleteCategoryAsync(int id);
    }
}
