using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStay.Domain.Entities;

namespace SmartStay.Infrastructure.Persistence.Configurations;

public class MeterReadingConfiguration : IEntityTypeConfiguration<MeterReading>
{
    public void Configure(EntityTypeBuilder<MeterReading> builder)
    {
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("ck_meter_readings_month_range", "month BETWEEN 1 AND 12");
            t.HasCheckConstraint("ck_meter_readings_year_min", "\"year\" >= 2000");
            t.HasCheckConstraint("ck_meter_readings_old_unit_non_negative", "old_unit >= 0");
            t.HasCheckConstraint("ck_meter_readings_new_unit_gte_old_unit", "new_unit >= old_unit");
        });

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Type).IsRequired().HasMaxLength(50);
        builder.Property(m => m.OldUnit).HasColumnType("numeric(18,2)");
        builder.Property(m => m.NewUnit).HasColumnType("numeric(18,2)");
        builder.Property(m => m.PhotoUrl).HasMaxLength(500);
        builder.HasIndex(m => m.RoomId);
        builder.HasIndex(m => new { m.RoomId, m.Type, m.Month, m.Year })
            .HasDatabaseName("ux_meter_readings_room_type_month_year_active")
            .IsUnique()
            .HasFilter("is_deleted = false");
        builder.HasOne(m => m.Room)
            .WithMany(r => r.MeterReadings)
            .HasForeignKey(m => m.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}
