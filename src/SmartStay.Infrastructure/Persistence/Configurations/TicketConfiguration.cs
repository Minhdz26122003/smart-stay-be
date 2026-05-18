using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStay.Domain.Entities;

namespace SmartStay.Infrastructure.Persistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Description).IsRequired().HasMaxLength(4000);
        builder.Property(t => t.Status).HasConversion<string>();
        builder.Property(t => t.Category).HasConversion<string>();
        builder.Property(t => t.Priority).HasConversion<string>();
        builder.Property(t => t.PhotoUrls).HasColumnType("text[]");
        builder.HasIndex(t => t.RoomId);
        builder.HasIndex(t => t.TenantId);
        builder.HasOne(t => t.Room)
            .WithMany(r => r.Tickets)
            .HasForeignKey(t => t.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(t => t.Tenant)
            .WithMany(u => u.TicketsAsTenant)
            .HasForeignKey(t => t.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}
