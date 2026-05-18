using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStay.Domain.Entities;

namespace SmartStay.Infrastructure.Persistence.Configurations;

public class ListingConfiguration : IEntityTypeConfiguration<Listing>
{
    public void Configure(EntityTypeBuilder<Listing> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.PhotoUrls).HasColumnType("text[]");
        builder.Property(l => l.Description).IsRequired().HasMaxLength(4000);
        builder.HasIndex(l => l.RoomId);
        builder.HasOne(l => l.Room)
            .WithMany(r => r.Listings)
            .HasForeignKey(l => l.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasQueryFilter(l => !l.IsDeleted);
    }
}
