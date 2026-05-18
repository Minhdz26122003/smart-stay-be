using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStay.Domain.Entities;

namespace SmartStay.Infrastructure.Persistence.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("ck_rooms_base_price_non_negative", "base_price >= 0");
            t.HasCheckConstraint("ck_rooms_area_positive", "area_m2 IS NULL OR area_m2 > 0");
            t.HasCheckConstraint("ck_rooms_max_occupants_positive", "max_occupants IS NULL OR max_occupants > 0");
        });

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name).IsRequired().HasMaxLength(100);
        builder.Property(r => r.Type).HasMaxLength(100);
        builder.Property(r => r.BasePrice).HasColumnType("numeric(18,2)");
        builder.Property(r => r.Status).HasConversion<string>();
        builder.HasIndex(r => r.PropertyId);
        builder.HasOne(r => r.Property)
            .WithMany(p => p.Rooms)
            .HasForeignKey(r => r.PropertyId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}
