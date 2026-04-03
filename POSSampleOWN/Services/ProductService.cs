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
    public class ProductService : IProductService
    {
        private readonly POSDbContext _db;

        public ProductService(POSDbContext db)
        {
            _db = db;
        }

        private IQueryable<Product> ActiveProductQuery => _db.Products
            .AsNoTracking()
            .Where(p => !p.DeleteFlag);

        #region get all products
        public async Task<List<ProductDTO>> GetAllAsync()
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
        public async Task<ProductDTO> GetByIdAsync(int id)
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
        public async Task<List<ProductDTO>> GetAvailableAsync()
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
        public async Task<ProductDTO> CreateAsync(CreateProductDTO request)
        {
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

        #region update product
        public async Task<ProductDTO> UpdateAsync(int id, UpdateProductDTO request)
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
        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product is null) throw new Exception("Product not found");

            product.DeleteFlag = true;
            product.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return true;
        }

        public Task<ApiResponse<List<ProductDTO>>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

