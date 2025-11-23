using HotelBookingSystem.Domain.Entities.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBookingSystem.Infrastructure.Data.Configurations.Bookings;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Booking");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.CheckInDateTime)
            .IsRequired();

        builder.Property(b => b.CheckOutDateTime)
            .IsRequired();

        builder.Property(b => b.NumOfAdults)
            .IsRequired();

        builder.Property(b => b.NumOfChildren)
            .IsRequired();

        builder.Property(b => b.SpecialRequests)
            .HasMaxLength(2000);

        builder.Property(b => b.ConfirmationCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(b => b.CreatedAt)
            .IsRequired();

        builder.HasIndex(b => b.GuestId);
        builder.HasIndex(b => b.HotelId);
        builder.HasIndex(b => b.HotelRoomId);

        builder.HasOne(b => b.Guest)
            .WithMany(g => g.Bookings)
            .HasForeignKey(b => b.GuestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Hotel)
            .WithMany(h => h.Bookings)
            .HasForeignKey(b => b.HotelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.HotelRoom)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.HotelRoomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
