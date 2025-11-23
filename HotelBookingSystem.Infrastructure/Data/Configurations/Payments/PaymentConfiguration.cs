using HotelBookingSystem.Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBookingSystem.Infrastructure.Data.Configurations.Payments;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payment");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PaymentAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.PaymentStatus)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(p => p.PaymentDate)
            .IsRequired();

        builder.HasIndex(p => p.BookingId);
        builder.HasIndex(p => p.DiscountId);
        builder.HasIndex(p => p.PaymentMethodId);

        builder.HasOne(p => p.Booking)
            .WithMany(b => b.Payments)
            .HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Discount)
            .WithMany(d => d.Payments)
            .HasForeignKey(p => p.DiscountId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.PaymentMethod)
            .WithMany(pm => pm.Payments)
            .HasForeignKey(p => p.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
