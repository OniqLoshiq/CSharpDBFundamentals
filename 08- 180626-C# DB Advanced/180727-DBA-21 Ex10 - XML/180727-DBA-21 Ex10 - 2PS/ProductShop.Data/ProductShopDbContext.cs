using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using ProductShop.Models;
using System;

namespace ProductShop.Data
{
    public class ProductShopDbContext : DbContext
    {
        public ProductShopDbContext()
        {
        }

        public ProductShopDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.ConfigurationString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryProduct>()
                .HasKey(x => new { x.ProductId, x.CategoryId });

            modelBuilder.Entity<CategoryProduct>()
                .HasOne(x => x.Product)
                .WithMany(x => x.CategoryProducts)
                .HasForeignKey(x => x.CategoryId);

            modelBuilder.Entity<CategoryProduct>()
                .HasOne(x => x.Category)
                .WithMany(x => x.CategoryProducts)
                .HasForeignKey(x => x.CategoryId);

            modelBuilder.Entity<User>(u =>
            {
                u.HasMany(x => x.ProductsSold)
                 .WithOne(x => x.Seller)
                 .HasForeignKey(x => x.SellerId);

                u.HasMany(x => x.ProductsBought)
                 .WithOne(x => x.Buyer)
                 .HasForeignKey(x => x.BuyerId);
            });
        }
    }
}
