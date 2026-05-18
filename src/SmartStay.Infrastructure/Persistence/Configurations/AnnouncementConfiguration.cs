using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStay.Domain.Entities;

namespace SmartStay.Infrastructure.Persistence.Configurations;

public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
{
    public void Configure(EntityTypeBuilder<Announcement> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Title).IsRequired().HasMaxLength(200);
        builder.Property(a => a.Content).IsRequired().HasMaxLength(4000);
        builder.HasIndex(a => a.PropertyId);
        builder.HasIndex(a => a.RoomId);
        builder.HasIndex(a => a.CreatedBy);
        builder.HasOne(a => a.Property)
            .WithMany(p => p.Announcements)
            .HasForeignKey(a => a.PropertyId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(a => a.Room)
            .WithMany(r => r.Announcements)
            .HasForeignKey(a => a.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(a => a.Creator)
            .WithMany(u => u.AnnouncementsCreated)
            .HasForeignKey(a => a.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}
