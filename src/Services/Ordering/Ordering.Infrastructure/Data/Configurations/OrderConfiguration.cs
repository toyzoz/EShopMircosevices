using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;

namespace Ordering.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(
            v => v.Value,
            v => OrderId.Of(v)
        );

        builder.HasOne<Customer>().WithMany().HasForeignKey(o => o.CustomerId).IsRequired();
        builder.HasMany(x => x.OrderItems).WithOne().HasForeignKey(o => o.OrderId).IsRequired();

        builder.ComplexProperty(
            o => o.OrderName, nameBuilder =>
            {
                nameBuilder.Property(n => n.Value)
                    .HasColumnName(nameof(Order.OrderName))
                    .HasMaxLength(100)
                    .IsRequired();
            });

        builder.ComplexProperty(x => x.ShippingAddress, addressBuilder =>
        {
            addressBuilder.Property(a => a.FirstName).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.LastName).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.EmailAddress).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.AddressLine).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.Country).HasMaxLength(180).IsRequired();
            addressBuilder.Property(a => a.State).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.ZipCode).HasMaxLength(50).IsRequired();
        });

        builder.ComplexProperty(x => x.BillingAddress, addressBuilder =>
        {
            addressBuilder.Property(a => a.FirstName).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.LastName).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.EmailAddress).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.AddressLine).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.Country).HasMaxLength(180).IsRequired();
            addressBuilder.Property(a => a.State).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.ZipCode).HasMaxLength(50).IsRequired();
        });

        builder.ComplexProperty(x => x.Payment, paymentBuilder =>
        {
            paymentBuilder.Property(p => p.CardName).HasMaxLength(50).IsRequired();
            paymentBuilder.Property(p => p.CardNumber).HasMaxLength(24).IsRequired();
            paymentBuilder.Property(p => p.Expiration).HasMaxLength(10).IsRequired();
            paymentBuilder.Property(p => p.CVV).HasMaxLength(3).IsRequired();
            paymentBuilder.Property(p => p.PaymentMethod);
        });

        builder.Property(o => o.Status)
            .HasDefaultValue(OrderStatus.Draft)
            .HasConversion(
                v => v.ToString(),
                v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v));

        builder.Property(o => o.TotalPrice);
    }
}