using System.Collections.Generic;
using System.Threading.Tasks;
using POSSampleOWN.DTOs;
using POSSampleOWN.Responses;

namespace POSSampleOWN.Services
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllAsync();
        Task<ProductDTO> GetByIdAsync(int id);
        Task<List<ProductDTO>> GetAvailableAsync();
        Task<ProductDTO> CreateAsync(CreateProductDTO request);
        Task<ProductDTO> UpdateAsync(int id, UpdateProductDTO request);
        Task<bool> DeleteAsync(int id);
    }
}
