using HotelBookingSystem.Domain.Entities.Discounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBookingSystem.Infrastructure.Data.Configurations.Discounts;

public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.ToTable("Discount");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.DiscountDescription)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(d => d.DiscountRate)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(d => d.IsActive)
            .IsRequired();
    }
}
