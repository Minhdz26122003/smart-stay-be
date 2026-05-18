using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStay.Domain.Entities;

namespace SmartStay.Infrastructure.Persistence.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.PlateNumber).IsRequired().HasMaxLength(20);
        builder.Property(v => v.PhotoUrl).HasMaxLength(500);
        builder.HasIndex(v => v.TenantId);
        builder.HasOne(v => v.Tenant)
            .WithMany(u => u.Vehicles)
            .HasForeignKey(v => v.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasQueryFilter(v => !v.IsDeleted);
    }
}
