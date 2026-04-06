using POSSampleOWN.DTOs;
using POSSampleOWN.Responses;

namespace POSSampleOWN.domain.Features.ProductsCatalog
{
    public interface IProductCatalogService
    {
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task<List<ProductDTO>> GetAvailableProductsAsync();
        Task<ProductDTO> CreateProductAsync(CreateProductDTO request);

        Task<List<ProductDTO>> BulkCreateProductsAsync(List<CreateProductDTO> request);
        Task<ProductDTO> UpdateProductAsync(int id, UpdateProductDTO request);
        Task<bool> DeleteProductAsync(int id);
        Task<List<ProductDTO>> GetProductsByTermAsync(string term);
        Task<List<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> GetCategoryByIdAsync(int id);
        Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO request);
        Task<CategoryDTO> UpdateCategoryAsync(int id, UpdateCategoryDTO request);
        Task<List<CategoryDTO>> GetCategoriesByTermAsync(string term);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
