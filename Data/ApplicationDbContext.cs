using Microsoft.EntityFrameworkCore;
using WoodSalesPlatform.Models;

namespace WoodSalesPlatform.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed admin user (password: admin123)
            // Pre-generated BCrypt hash for "admin123" to avoid dynamic values in HasData
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = "$2a$11$J3eqC5rJKVMZXJGYHbPaVeHXn4PZ1pT8qYhfZGOQZZLKQ.WQ.8.Oi",
                    IsAdmin = true
                }
            );

            modelBuilder.Entity<Product>().HasMany(o => o.Orders).WithOne(p => p.Product).OnDelete(DeleteBehavior.ClientNoAction);

        }
    }
}
