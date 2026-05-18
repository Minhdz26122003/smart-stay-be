using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStay.Domain.Entities;

namespace SmartStay.Infrastructure.Persistence.Configurations;

public class VisitorLogConfiguration : IEntityTypeConfiguration<VisitorLog>
{
    public void Configure(EntityTypeBuilder<VisitorLog> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.VisitorName).IsRequired().HasMaxLength(100);
        builder.Property(v => v.Phone).IsRequired().HasMaxLength(20);
        builder.HasIndex(v => v.TenantId);
        builder.HasOne(v => v.Tenant)
            .WithMany(u => u.VisitorLogs)
            .HasForeignKey(v => v.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasQueryFilter(v => !v.IsDeleted);
    }
}
