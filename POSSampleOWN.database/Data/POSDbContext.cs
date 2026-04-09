using Microsoft.EntityFrameworkCore;
using POSSampleOWN.database.Models;
using POSSampleOWN.Models;

namespace POSSampleOWN.Data
{
    public class POSDbContext: DbContext
    {
        public POSDbContext(DbContextOptions<POSDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tbl_Product> Products { get; set; }
        public DbSet<Tbl_Category> Categories { get; set; }
        public DbSet<Tbl_User> Users { get; set; }
        public DbSet<Tbl_Sale> Sales { get; set; }
        public DbSet<Tbl_SaleItem> SaleItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Product>()
            //     .HasOne(p => p.User)
            //     .WithMany(u => u.Products)
            //     .HasForeignKey(p => p.CreatedBy);

            modelBuilder.Entity<Tbl_User>().ToTable("Tbl_User");
            modelBuilder.Entity<Tbl_Product>().ToTable("Tbl_Product");
            modelBuilder.Entity<Tbl_Category>().ToTable("Tbl_Category");
            modelBuilder.Entity<Tbl_Sale>().ToTable("Tbl_Sale");
            modelBuilder.Entity<Tbl_SaleItem>().ToTable("Tbl_SaleItem");

            modelBuilder.Entity<Tbl_Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
