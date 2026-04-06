using Microsoft.EntityFrameworkCore;
using POSSampleOWN.Models;

namespace POSSampleOWN.Data
{
    public class POSDbContext: DbContext
    {
        public POSDbContext(DbContextOptions<POSDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Product>()
            //     .HasOne(p => p.User)
            //     .WithMany(u => u.Products)
            //     .HasForeignKey(p => p.CreatedBy);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
