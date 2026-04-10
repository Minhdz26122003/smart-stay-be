using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStay.Domain.Entities;

namespace SmartStay.Infrastructure.Persistence.Configurations;

public class OtpCodeConfiguration : IEntityTypeConfiguration<OtpCode>
{
    public void Configure(EntityTypeBuilder<OtpCode> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(256);
        builder.Property(x => x.CodeHash).IsRequired();
        builder.Property(x => x.Purpose).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ExpiresAt).IsRequired();
        builder.HasIndex(x => x.Email);
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
