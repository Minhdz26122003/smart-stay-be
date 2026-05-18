using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStay.Domain.Entities;

namespace SmartStay.Infrastructure.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Note).HasMaxLength(1000);
        builder.HasIndex(b => b.ListingId);
        builder.HasIndex(b => b.GuestId);
        builder.HasOne(b => b.Listing)
            .WithMany(l => l.Bookings)
            .HasForeignKey(b => b.ListingId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(b => b.Guest)
            .WithMany(u => u.BookingsAsGuest)
            .HasForeignKey(b => b.GuestId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasQueryFilter(b => !b.IsDeleted);
    }
}
