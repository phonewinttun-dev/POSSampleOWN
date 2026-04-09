using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using POSSampleOWN.domain.Features.ProductsCatalog;
using POSSampleOWN.domain.Features.Search;
using POSSampleOWN.domain.Features.Inventory;
using POSSampleOWN.domain.Features.Sale;

namespace POSSampleOWN.domain.Features
{
    public static class FeaturesManager
    {
        public static void AddDomain(this WebApplicationBuilder builder)
        {
            
            builder.Services.AddDbContext<POSSampleOWN.Data.POSDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("POSConnectionString")));
            
            // Register Features
            builder.Services.AddScoped<IProductCatalogService, ProductCatalogService>();
            builder.Services.AddScoped<ISearchService, SearchService>();
            builder.Services.AddScoped<IInventoryService, InventoryService>();
            builder.Services.AddScoped<ISaleService, SaleService>();
        }
    }
}
