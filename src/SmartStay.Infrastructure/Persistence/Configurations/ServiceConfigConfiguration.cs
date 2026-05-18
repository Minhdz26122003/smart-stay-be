using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStay.Domain.Entities;

namespace SmartStay.Infrastructure.Persistence.Configurations;

public class ServiceConfigConfiguration : IEntityTypeConfiguration<ServiceConfig>
{
    public void Configure(EntityTypeBuilder<ServiceConfig> builder)
    {
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("ck_service_configs_unit_price_non_negative", "unit_price >= 0");
        });

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Type).IsRequired().HasMaxLength(50);
        builder.Property(s => s.UnitPrice).HasColumnType("numeric(18,2)");
        builder.HasIndex(s => s.PropertyId);
        builder.HasIndex(s => s.RoomId);
        builder.HasOne(s => s.Property)
            .WithMany(p => p.ServiceConfigs)
            .HasForeignKey(s => s.PropertyId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(s => s.Room)
            .WithMany(r => r.ServiceConfigs)
            .HasForeignKey(s => s.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}
