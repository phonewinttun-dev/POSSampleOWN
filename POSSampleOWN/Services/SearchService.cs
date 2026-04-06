using Microsoft.EntityFrameworkCore;
using POSSampleOWN.Data;
using POSSampleOWN.DTOs;

namespace POSSampleOWN.Services
{
    public class SearchService : ISearchService
    {
        private readonly POSDbContext _dbContext;

        public SearchService(POSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SearchDTO>> GeneralSearchAsync(string term)
        {
            var searchTerm = term.ToLower();

            var products = await _dbContext.Products
                .Where(p => p.Name.ToLower().Contains(searchTerm) || p.Description.ToLower().Contains(searchTerm))
                .Select(p => new SearchDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Type = "Product",
                    Price = p.Price
                })
                .ToListAsync();

            var categories = await _dbContext.Categories
                .Where(c => c.Name.ToLower().Contains(searchTerm) || c.Description.ToLower().Contains(searchTerm))
                .Select(c => new SearchDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = "Category",
                    Price = null
                })
                .ToListAsync();

            return products.Concat(categories).ToList();

        }
    }
}
