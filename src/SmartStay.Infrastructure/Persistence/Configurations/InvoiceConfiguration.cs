using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStay.Domain.Entities;

namespace SmartStay.Infrastructure.Persistence.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("ck_invoices_month_range", "month BETWEEN 1 AND 12");
            t.HasCheckConstraint("ck_invoices_year_min", "\"year\" >= 2000");
            t.HasCheckConstraint("ck_invoices_total_amount_non_negative", "total_amount >= 0");
            t.HasCheckConstraint("ck_invoices_paid_amount_range", "paid_amount >= 0 AND paid_amount <= total_amount");
        });

        builder.HasKey(i => i.Id);
        builder.Property(i => i.TotalAmount).HasColumnType("numeric(18,2)");
        builder.Property(i => i.PaidAmount).HasColumnType("numeric(18,2)");
        // Store invoice breakdown as JSONB for flexible structure
        builder.Property(i => i.BreakdownJson).HasColumnType("jsonb").IsRequired();
        builder.Property(i => i.Status).HasConversion<string>();
        builder.HasIndex(i => i.RoomId);
        builder.HasIndex(i => i.ContractId);
        builder.HasIndex(i => new { i.ContractId, i.Month, i.Year })
            .HasDatabaseName("ux_invoices_contract_id_month_year_active")
            .IsUnique()
            .HasFilter("is_deleted = false");
        builder.HasOne(i => i.Room)
            .WithMany(r => r.Invoices)
            .HasForeignKey(i => i.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(i => i.Contract)
            .WithMany(c => c.Invoices)
            .HasForeignKey(i => i.ContractId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasQueryFilter(i => !i.IsDeleted);
    }
}
