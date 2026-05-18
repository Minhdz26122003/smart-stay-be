using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStay.Domain.Entities;

namespace SmartStay.Infrastructure.Persistence.Configurations;

public class RoommateConfiguration : IEntityTypeConfiguration<Roommate>
{
    public void Configure(EntityTypeBuilder<Roommate> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.FullName).IsRequired().HasMaxLength(100);
        builder.Property(r => r.Phone).IsRequired().HasMaxLength(20);
        builder.Property(r => r.CccdPhotoUrl).HasMaxLength(500);
        builder.HasIndex(r => r.ContractId);
        builder.HasOne(r => r.Contract)
            .WithMany(c => c.Roommates)
            .HasForeignKey(r => r.ContractId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}
