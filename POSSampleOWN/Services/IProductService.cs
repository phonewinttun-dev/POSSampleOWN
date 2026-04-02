using System.Collections.Generic;
using System.Threading.Tasks;
using POSSampleOWN.DTOs;
using POSSampleOWN.Responses;

namespace POSSampleOWN.Services
{
    public interface IProductService
    {
        Task<ApiResponse<List<ProductDTO>>> GetAllAsync();
        Task<ApiResponse<ProductDTO>> GetByIdAsync(int id);
        Task<ApiResponse<List<ProductDTO>>> GetAvailableProductsAsync();
        Task<ApiResponse<List<ProductDTO>>> GetAllProductsAsync();
        Task<ApiResponse<ProductDTO>> CreateAsync(CreateProductDTO request);
        Task<ApiResponse<ProductDTO>> UpdateAsync(int id, UpdateProductDTO request);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
