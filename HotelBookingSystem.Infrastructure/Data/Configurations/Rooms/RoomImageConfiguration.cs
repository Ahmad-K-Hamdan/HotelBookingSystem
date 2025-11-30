using HotelBookingSystem.Domain.Entities.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBookingSystem.Infrastructure.Data.Configurations.Rooms;

public class RoomTypeImageConfiguration : IEntityTypeConfiguration<RoomTypeImage>
{
    public void Configure(EntityTypeBuilder<RoomTypeImage> builder)
    {
        builder.ToTable("RoomTypeImage");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Url)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(i => i.IsMain)
            .IsRequired();

        builder.HasIndex(i => i.HotelRoomTypeId);

        builder.HasOne(i => i.HotelRoomType)
            .WithMany(rt => rt.Images)
            .HasForeignKey(i => i.HotelRoomTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
