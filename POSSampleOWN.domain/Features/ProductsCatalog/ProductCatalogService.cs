using Microsoft.EntityFrameworkCore;
using POSSampleOWN.Data;
using POSSampleOWN.DTOs;
using POSSampleOWN.Models;
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

        private IQueryable<Product> ActiveProductQuery => _db.Products
            .AsNoTracking()
            .Where(p => !p.DeleteFlag);

        #region get all products
        public async Task<List<ProductDTO>> GetAllProductsAsync()
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

            return products;
        }

        #endregion

        #region get active products by id
        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var product = await ActiveProductQuery
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null) throw new Exception("Product not found");

            var data = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId
            };

            return data;
        }
        #endregion

        #region get available products
        public async Task<List<ProductDTO>> GetAvailableProductsAsync()
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

            return products;
        }
        #endregion

        #region create product
        public async Task<ProductDTO> CreateProductAsync(CreateProductDTO request)
        {
            var categoryExists = await _db.Categories
                .AnyAsync(c => c.Id == request.CategoryId && !c.DeleteFlag);

            if (!categoryExists) throw new Exception("Category not found");

            var newProduct = new Product
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

            return data;
        }
        #endregion

        #region bulk insert product
        public async Task<List<ProductDTO>> BulkCreateProductsAsync(List<CreateProductDTO> request)
        {
            var products = request.Select(p => new Product
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

            return products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                DeleteFlag = p.DeleteFlag
            }).ToList();
        }
        #endregion

        #region update product
        public async Task<ProductDTO> UpdateProductAsync(int id, UpdateProductDTO request)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product is null || product.DeleteFlag == true)
                throw new Exception("Product not found");

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

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId
            };
        }
        #endregion

        #region delete product
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product is null) throw new Exception("Product not found");

            product.DeleteFlag = true;
            product.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return true;
        }
        #endregion

        #region Search Products By Term
        public async Task<List<ProductDTO>> GetProductsByTermAsync(string term)
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

            return products;
        }
        #endregion

        #region get all categories
        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
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
        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
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
        public async Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO request)
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
        public async Task<CategoryDTO> UpdateCategoryAsync(int id, UpdateCategoryDTO request)
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
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _db.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category is null)
                throw new Exception("Category not found!");

            if (category.Products != null && category.Products.Any(p => !p.DeleteFlag))
                throw new Exception("Cannot delete category with existing products.");

            category.DeleteFlag = true;

            await _db.SaveChangesAsync();

            return true;
        }
        #endregion

        #region get categories by term
        public async Task<List<CategoryDTO>> GetCategoriesByTermAsync(string term)
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

            return categories;
        }
        #endregion
    }
}
