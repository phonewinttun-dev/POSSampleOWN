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
        public async Task<ApiResponse<List<CategoryDTO>>> GetAllCategoriesAsync()
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

            return ApiResponse<List<CategoryDTO>>.Success(categories, "Categories retrieved successfully.");
        }
        #endregion

        #region get category by id
        public async Task<ApiResponse<CategoryDTO>> GetByIdAsync(int id)
        {
            var category = await _db.Categories
                .AsNoTracking()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category is null)
            {
                return ApiResponse<CategoryDTO>.Fail("Category not found.");
            }

            var data = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return ApiResponse<CategoryDTO>.Success(data, "Category retrieved successfully.");
        }
        #endregion

        #region create category
        public async Task<ApiResponse<CategoryDTO>> CreateAsync(CreateCategoryDTO request)
        {
            if (request is null)
            {
                return ApiResponse<CategoryDTO>.Fail("Category cannot be null.");
            }

            var newCategory = new Category
            {
                Name = request.Name.Trim(),
                Description = request.Description!.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _db.Categories.Add(newCategory);

            var result = await _db.SaveChangesAsync();

            if (result <= 0)
            {
                return ApiResponse<CategoryDTO>.Fail("Failed to create category.");
            }

            var data = new CategoryDTO
            {
                Id = newCategory.Id,
                Name = newCategory.Name,
                Description = newCategory.Description
            };

            return ApiResponse<CategoryDTO>.Success(data, "Category created successfully.");
        }
        #endregion

        #region update category
        public async Task<ApiResponse<CategoryDTO>> UpdateAsync(int id, UpdateCategoryDTO request)
        {
            if (request is null)
            {
                return ApiResponse<CategoryDTO>.Fail("Category cannot be null.");
            }

            var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
            
            if (category is null)
            {
                return ApiResponse<CategoryDTO>.Fail("Category not found.");
            }

            if (!string.IsNullOrEmpty(request.Name))
                category.Name = request.Name;

            if (request.Description != null)
                category.Description = request.Description;

            var result = await _db.SaveChangesAsync();

            if (result <= 0)
            {
                return ApiResponse<CategoryDTO>.Fail("Failed to update category.");
            }

            var data = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return ApiResponse<CategoryDTO>.Success(data, "Category updated successfully.");
        }
        #endregion

        #region delete category
        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var category = await _db.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category is null)
            {
                return ApiResponse<bool>.Fail("Category not found.");
            }

            if (category.Products != null && category.Products.Any(p => !p.DeleteFlag))
            {
                return ApiResponse<bool>.Fail("Category cannot be deleted because it contains active products.");
            }

            category.DeleteFlag = true;

            var success = await _db.SaveChangesAsync();

            if (success <= 0)
            {
                return ApiResponse<bool>.Fail("Failed to delete category.");
            }

            return ApiResponse<bool>.Success(true, "Category deleted successfully.");
        }
        #endregion
    }
}
