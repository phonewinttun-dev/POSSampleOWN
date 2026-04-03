using Microsoft.EntityFrameworkCore;
using POSSampleOWN.Data;
using POSSampleOWN.DTOs;
using POSSampleOWN.Models;
using POSSampleOWN.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSSampleOWN.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly POSDbContext _db;

        public CategoryService(POSDbContext db)
        {
            _db = db;
        }

        #region get all categories
        public async Task<List<CategoryDTO>> GetAllAsync()
        {
            var categories = await _db.Categories
                .AsNoTracking()
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync();

            return categories;
        }
        #endregion

        #region get category by id
        public async Task<CategoryDTO> GetByIdAsync(int id)
        {
            var category = await _db.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category is null) throw new Exception("Category not found.");


            var data = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return data;
        }
        #endregion

        #region create category
        public async Task<CategoryDTO> CreateAsync(CreateCategoryDTO request)
        {

            var newCategory = new Category
            {
                Name = request.Name.Trim(),
                Description = request.Description!.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _db.Categories.Add(newCategory);

            await _db.SaveChangesAsync();

            var data = new CategoryDTO
            {
                Id = newCategory.Id,
                Name = newCategory.Name,
                Description = newCategory.Description
            };

            return data;
        }
        #endregion

        #region update category
        public async Task<CategoryDTO> UpdateAsync(int id, UpdateCategoryDTO request)
        {

            var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category is null) throw new Exception("Category not found.");

            if (!string.IsNullOrWhiteSpace(request.Name))
                category.Name = request.Name.Trim();

            if (!string.IsNullOrWhiteSpace(request.Description))
                category.Description = request.Description.Trim();

            await _db.SaveChangesAsync();

            var data = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return data;
        }
        #endregion

        #region delete category
        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _db.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category is null) throw new Exception("Category not found!");

            category.DeleteFlag = true;

            var success = await _db.SaveChangesAsync();

            return true;
        }
        #endregion
    }
}
