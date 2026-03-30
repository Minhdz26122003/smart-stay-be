using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStay.Domain.Entities;

namespace SmartStay.Infrastructure.Persistence.Configurations;

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.ItemName).IsRequired().HasMaxLength(200);
        builder.Property(i => i.CheckInPhotos).HasColumnType("text[]");
        builder.Property(i => i.CheckOutPhotos).HasColumnType("text[]");
        builder.Property(i => i.Condition).HasConversion<string>();
        builder.HasQueryFilter(i => !i.IsDeleted);
    }
}
