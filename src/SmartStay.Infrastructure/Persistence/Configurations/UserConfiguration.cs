using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStay.Domain.Entities;

namespace SmartStay.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.FullName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Phone).IsRequired().HasMaxLength(20);
        builder.HasIndex(u => u.Phone).IsUnique().HasFilter("is_deleted = false");
        builder.Property(u => u.Email).HasMaxLength(200);
        builder.HasIndex(u => u.Email).IsUnique().HasFilter("is_deleted = false AND email IS NOT NULL");
        builder.Property(u => u.PasswordHash).IsRequired();
        // Store roles as a PostgreSQL text[] array column
        builder.Property(u => u.Roles)
            .HasColumnType("text[]");
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}
