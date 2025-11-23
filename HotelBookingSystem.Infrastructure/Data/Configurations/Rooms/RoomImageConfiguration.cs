using HotelBookingSystem.Domain.Entities.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBookingSystem.Infrastructure.Data.Configurations.Rooms;

public class RoomImageConfiguration : IEntityTypeConfiguration<RoomImage>
{
    public void Configure(EntityTypeBuilder<RoomImage> builder)
    {
        builder.ToTable("RoomImage");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Url)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(i => i.IsMain)
            .IsRequired();

        builder.HasIndex(i => i.HotelRoomId);

        builder.HasOne(i => i.HotelRoom)
            .WithMany(r => r.Images)
            .HasForeignKey(i => i.HotelRoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
