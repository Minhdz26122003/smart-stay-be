using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStay.Domain.Entities;
using SmartStay.Domain.ValueObjects;

namespace SmartStay.Infrastructure.Persistence.Configurations;

public class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.OwnsOne(p => p.Address, a =>
        {
            a.Property(x => x.Street).HasColumnName("address_street").HasMaxLength(200);
            a.Property(x => x.Ward).HasColumnName("address_ward").HasMaxLength(100);
            a.Property(x => x.District).HasColumnName("address_district").HasMaxLength(100);
            a.Property(x => x.City).HasColumnName("address_city").HasMaxLength(100);
        });
        builder.Property(p => p.SharedAmenities).HasColumnType("text[]");
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
