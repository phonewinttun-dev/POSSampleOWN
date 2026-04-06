using System.Collections.Generic;
using System.Threading.Tasks;
using POSSampleOWN.DTOs;
using POSSampleOWN.Responses;

namespace POSSampleOWN.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO> GetByIdAsync(int id);
        Task<CategoryDTO> CreateAsync(CreateCategoryDTO request);
        Task<CategoryDTO> UpdateAsync(int id, UpdateCategoryDTO request);
        Task<List<CategoryDTO>> GetCategoriesByTermAsync(string term);
        Task<bool> DeleteAsync(int id);
    }
}
