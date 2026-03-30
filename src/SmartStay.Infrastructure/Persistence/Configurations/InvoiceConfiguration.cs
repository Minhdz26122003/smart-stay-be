using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStay.Domain.Entities;

namespace SmartStay.Infrastructure.Persistence.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.TotalAmount).HasColumnType("numeric(18,2)");
        builder.Property(i => i.PaidAmount).HasColumnType("numeric(18,2)");
        // Store invoice breakdown as JSONB for flexible structure
        builder.Property(i => i.BreakdownJson).HasColumnType("jsonb").IsRequired();
        builder.Property(i => i.Status).HasConversion<string>();
        builder.HasQueryFilter(i => !i.IsDeleted);
    }
}
