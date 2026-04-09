using Microsoft.EntityFrameworkCore;
using POSSampleOWN.Data;
using POSSampleOWN.DTOs;
using POSSampleOWN.Models;
using POSSampleOWN.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSampleOWN.domain.Features.ProductsCatalog
{
    public class ProductCatalogService: IProductCatalogService
    {
        private readonly POSDbContext _db;

        public ProductCatalogService(POSDbContext db)
        {
            _db = db;
        }

        private IQueryable<Tbl_Product> ActiveProductQuery => _db.Products
            .AsNoTracking()
            .Where(p => !p.DeleteFlag);

        #region get all products
        public async Task<ApiResponse<List<ProductDTO>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _db.Products
                    .AsNoTracking()
                    .Select(p => new ProductDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        StockQuantity = p.StockQuantity,
                        CategoryId = p.CategoryId,
                        DeleteFlag = p.DeleteFlag
                    })
                    .ToListAsync();

                return ApiResponse<List<ProductDTO>>.Success(products);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ProductDTO>>.Fail(ex.Message);
            }
        }

        #endregion

        #region get active products by id
        public async Task<ApiResponse<ProductDTO>> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await ActiveProductQuery
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product is null) return ApiResponse<ProductDTO>.Fail("Product not found");

                var data = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    CategoryId = product.CategoryId
                };

                return ApiResponse<ProductDTO>.Success(data);
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductDTO>.Fail(ex.Message);
            }
        }
        #endregion

        #region get available products
        public async Task<ApiResponse<List<ProductDTO>>> GetAvailableProductsAsync()
        {
            try
            {
                var products = await ActiveProductQuery
                    .Where(p => p.StockQuantity > 0)
                    .Select(p => new ProductDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        StockQuantity = p.StockQuantity,
                        CategoryId = p.CategoryId
                    })
                    .ToListAsync();

                return ApiResponse<List<ProductDTO>>.Success(products);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ProductDTO>>.Fail(ex.Message);
            }
        }
        #endregion

        #region create product
        public async Task<ApiResponse<ProductDTO>> CreateProductAsync(CreateProductDTO request)
        {
            try
            {
                var categoryExists = await _db.Categories
                    .AnyAsync(c => c.Id == request.CategoryId && !c.DeleteFlag);

                if (!categoryExists) return ApiResponse<ProductDTO>.Fail("Category not found");

                var newProduct = new Tbl_Product
                {
                    Name = request.Name.Trim(),
                    Description = request.Description?.Trim(),
                    Price = request.Price,
                    StockQuantity = request.StockQuantity,
                    CategoryId = request.CategoryId,
                    CreatedAt = DateTime.UtcNow
                };

                _db.Products.Add(newProduct);

                await _db.SaveChangesAsync();

                var data = new ProductDTO
                {
                    Id = newProduct.Id,
                    Name = newProduct.Name,
                    Description = newProduct.Description,
                    Price = newProduct.Price,
                    StockQuantity = newProduct.StockQuantity,
                    CategoryId = newProduct.CategoryId,
                    DeleteFlag = newProduct.DeleteFlag
                };

                return ApiResponse<ProductDTO>.Success(data, "Product created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductDTO>.Fail(ex.Message);
            }
        }
        #endregion

        #region bulk insert product
        public async Task<ApiResponse<List<ProductDTO>>> BulkCreateProductsAsync(List<CreateProductDTO> request)
        {
            try
            {
                var products = request.Select(p => new Tbl_Product
                {
                    Name = p.Name.Trim(),
                    Description = p.Description?.Trim(),
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryId = p.CategoryId,
                    CreatedAt = DateTime.UtcNow
                }).ToList();

                _db.Products.AddRange(products);
                await _db.SaveChangesAsync();

                var data = products.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryId = p.CategoryId,
                    DeleteFlag = p.DeleteFlag
                }).ToList();

                return ApiResponse<List<ProductDTO>>.Success(data, $"{data.Count} products created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ProductDTO>>.Fail(ex.Message);
            }
        }
        #endregion

        #region update product
        public async Task<ApiResponse<ProductDTO>> UpdateProductAsync(int id, UpdateProductDTO request)
        {
            try
            {
                var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);

                if (product is null || product.DeleteFlag == true)
                    return ApiResponse<ProductDTO>.Fail("Product not found");

                if (request.Name != null)
                    product.Name = request.Name.Trim();

                if (request.Description != null)
                    product.Description = request.Description.Trim();

                if (request.Price != null)
                    product.Price = request.Price.Value;

                if (request.StockQuantity != null)
                    product.StockQuantity = request.StockQuantity.Value;

                if (request.CategoryId != null)
                    product.CategoryId = request.CategoryId.Value;

                product.UpdatedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync();

                var data = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    CategoryId = product.CategoryId
                };

                return ApiResponse<ProductDTO>.Success(data, "Product updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductDTO>.Fail(ex.Message);
            }
        }
        #endregion

        #region delete product
        public async Task<ApiResponse<bool>> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);

                if (product is null) return ApiResponse<bool>.Fail("Product not found");

                product.DeleteFlag = true;
                product.UpdatedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync();

                return ApiResponse<bool>.Success(true, "Product deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }
        #endregion

        #region Search Products By Term
        public async Task<ApiResponse<List<ProductDTO>>> GetProductsByTermAsync(string term)
        {
            try
            {
                var products = await ActiveProductQuery
                    .Where(p => p.Name.Contains(term) || (p.Description != null && p.Description.Contains(term)))
                    .Select(p => new ProductDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        StockQuantity = p.StockQuantity,
                        CategoryId = p.CategoryId
                    })
                    .ToListAsync();

                return ApiResponse<List<ProductDTO>>.Success(products);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ProductDTO>>.Fail(ex.Message);
            }
        }
        #endregion

        #region get all categories
        public async Task<ApiResponse<List<CategoryDTO>>> GetAllCategoriesAsync()
        {
            try
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

                return ApiResponse<List<CategoryDTO>>.Success(categories);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CategoryDTO>>.Fail(ex.Message);
            }
        }
        #endregion

        #region get category by id
        public async Task<ApiResponse<CategoryDTO>> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _db.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category is null) return ApiResponse<CategoryDTO>.Fail("Category not found.");


                var data = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };

                return ApiResponse<CategoryDTO>.Success(data);
            }
            catch (Exception ex)
            {
                return ApiResponse<CategoryDTO>.Fail(ex.Message);
            }
        }
        #endregion

        #region create category
        public async Task<ApiResponse<CategoryDTO>> CreateCategoryAsync(CreateCategoryDTO request)
        {
            try
            {
                var newCategory = new Tbl_Category
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

                return ApiResponse<CategoryDTO>.Success(data, "Category created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<CategoryDTO>.Fail(ex.Message);
            }
        }
        #endregion

        #region update category
        public async Task<ApiResponse<CategoryDTO>> UpdateCategoryAsync(int id, UpdateCategoryDTO request)
        {
            try
            {
                var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);

                if (category is null) return ApiResponse<CategoryDTO>.Fail("Category not found.");

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

                return ApiResponse<CategoryDTO>.Success(data, "Category updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<CategoryDTO>.Fail(ex.Message);
            }
        }
        #endregion

        #region delete category
        public async Task<ApiResponse<bool>> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _db.Categories
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category is null)
                    return ApiResponse<bool>.Fail("Category not found!");

                if (category.Products != null && category.Products.Any(p => !p.DeleteFlag))
                    return ApiResponse<bool>.Fail("Cannot delete category with existing products.");

                category.DeleteFlag = true;

                await _db.SaveChangesAsync();

                return ApiResponse<bool>.Success(true, "Category deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }
        #endregion

        #region get categories by term
        public async Task<ApiResponse<List<CategoryDTO>>> GetCategoriesByTermAsync(string term)
        {
            try
            {
                var categories = await _db.Categories
                    .AsNoTracking()
                    .Where(c => c.Name.Contains(term) || (c.Description != null && c.Description.Contains(term)))
                    .Select(c => new CategoryDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    })
                    .ToListAsync();

                return ApiResponse<List<CategoryDTO>>.Success(categories);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CategoryDTO>>.Fail(ex.Message);
            }
        }
        #endregion
    }
}
