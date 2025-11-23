using HotelBookingSystem.Domain.Entities.Hotels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBookingSystem.Infrastructure.Data.Configurations.Hotels;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.ToTable("Hotel");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.HotelName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(h => h.HotelAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(h => h.StarRating)
            .IsRequired();

        builder.Property(h => h.Latitude)
            .HasColumnType("decimal(9,6)");

        builder.Property(h => h.Longitude)
            .HasColumnType("decimal(9,6)");

        builder.Property(h => h.Description)
            .HasMaxLength(4000);

        builder.Property(h => h.CreatedAt)
            .IsRequired();

        builder.Property(h => h.UpdatedAt)
            .IsRequired(false);

        builder.HasIndex(h => h.CityId);
        builder.HasIndex(h => h.HotelGroupId);
        builder.HasIndex(h => h.DiscountId);

        builder.HasOne(h => h.City)
            .WithMany(c => c.Hotels)
            .HasForeignKey(h => h.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(h => h.HotelGroup)
            .WithMany(g => g.Hotels)
            .HasForeignKey(h => h.HotelGroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(h => h.Discount)
            .WithMany(d => d.Hotels)
            .HasForeignKey(h => h.DiscountId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
