using HotelBookingSystem.Domain.Entities.Hotels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBookingSystem.Infrastructure.Data.Configurations.Hotels;

public class HotelGroupConfiguration : IEntityTypeConfiguration<HotelGroup>
{
    public void Configure(EntityTypeBuilder<HotelGroup> builder)
    {
        builder.ToTable("HotelGroup");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.GroupName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(g => g.Description)
            .HasMaxLength(2000);

        builder.Property(g => g.CreatedAt)
            .IsRequired();

        builder.Property(g => g.UpdatedAt)
            .IsRequired(false);
    }
}
