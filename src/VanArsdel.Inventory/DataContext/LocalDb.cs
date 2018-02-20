using System;

using Microsoft.EntityFrameworkCore;

namespace VanArsdel.Inventory.Data
{
    public class LocalDb : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=VanArsdel.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubCategory>().HasKey(e => new { e.CategoryID, e.SubCategoryID });
            modelBuilder.Entity<OrderItem>().HasKey(e => new { e.OrderID, e.OrderLine });
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<CountryCode> CountryCodes { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<TaxType> TaxTypes { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
    }
}
