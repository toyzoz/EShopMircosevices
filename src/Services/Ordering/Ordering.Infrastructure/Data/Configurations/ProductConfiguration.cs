﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ordering.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasConversion(
            v => v.Value,
            v => ProductId.Of(v)
        );

        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
    }
}