using HotelBookingSystem.Domain.Entities.Visits;
using HotelBookingSystem.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBookingSystem.Infrastructure.Data.Configurations.Visits;

public class VisitLogConfiguration : IEntityTypeConfiguration<VisitLog>
{
    public void Configure(EntityTypeBuilder<VisitLog> builder)
    {
        builder.ToTable("VisitLog");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(v => v.VisitedAt)
            .IsRequired();

        builder.HasIndex(v => v.UserId);
        builder.HasIndex(v => v.HotelId);

        builder.HasOne(v => v.Hotel)
            .WithMany()
            .HasForeignKey(v => v.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
