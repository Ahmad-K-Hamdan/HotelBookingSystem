using HotelBookingSystem.Domain.Entities.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBookingSystem.Infrastructure.Data.Configurations.Rooms;

public class HotelRoomTypeConfiguration : IEntityTypeConfiguration<HotelRoomType>
{
    public void Configure(EntityTypeBuilder<HotelRoomType> builder)
    {
        builder.ToTable("HotelRoomType");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Description)
            .HasMaxLength(2000);

        builder.Property(r => r.PricePerNight)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(t => t.BedsCount)
            .IsRequired();

        builder.Property(t => t.MaxNumOfGuestsAdults)
            .IsRequired();

        builder.Property(t => t.MaxNumOfGuestsChildren)
            .IsRequired();

        builder.HasIndex(t => t.HotelId);

        builder.HasOne(t => t.Hotel)
            .WithMany(h => h.RoomTypes)
            .HasForeignKey(t => t.HotelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
