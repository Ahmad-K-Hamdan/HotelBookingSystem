using HotelBookingSystem.Domain.Entities.Guests;
using HotelBookingSystem.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBookingSystem.Infrastructure.Data.Configurations.Guests;

public class GuestConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        builder.ToTable("Guest");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(g => g.PassportNumber)
            .HasMaxLength(50);

        builder.Property(g => g.HomeCountry)
            .HasMaxLength(200);

        builder.HasIndex(g => g.UserId)
            .IsUnique();

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<Guest>(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
