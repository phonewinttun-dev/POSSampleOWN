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
        }
    }
}
