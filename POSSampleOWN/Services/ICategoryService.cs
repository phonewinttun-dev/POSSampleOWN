using System.Collections.Generic;
using System.Threading.Tasks;
using POSSampleOWN.DTOs;
using POSSampleOWN.Responses;

namespace POSSampleOWN.Services
{
    public interface ICategoryService
    {
        Task<ApiResponse<List<CategoryDTO>>> GetAllCategoriesAsync();
        Task<ApiResponse<CategoryDTO>> GetByIdAsync(int id);
        Task<ApiResponse<CategoryDTO>> CreateAsync(CreateCategoryDTO request);
        Task<ApiResponse<CategoryDTO>> UpdateAsync(int id, UpdateCategoryDTO request);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
