﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.Data.EntityConfig
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.ProductId);

            builder.Property(x => x.Name)
                   .HasMaxLength(50)
                   .IsUnicode(true);

            builder.HasMany(x => x.Sales)
                   .WithOne(x => x.Product)
                   .HasForeignKey(x => x.ProductId);

            builder.Property(x => x.Description)
                   .HasMaxLength(250)
                   .HasDefaultValue<string>("No description")
                   .IsUnicode(true);
        }
    }
}
