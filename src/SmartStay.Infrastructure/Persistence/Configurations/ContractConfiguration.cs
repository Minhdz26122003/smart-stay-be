using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStay.Domain.Entities;

namespace SmartStay.Infrastructure.Persistence.Configurations;

public class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("ck_contracts_end_after_start", "end_date > start_date");
            t.HasCheckConstraint("ck_contracts_deposit_non_negative", "deposit_amount >= 0");
        });

        builder.HasKey(c => c.Id);
        builder.Property(c => c.DepositAmount).HasColumnType("numeric(18,2)");
        builder.Property(c => c.Status).HasConversion<string>();
        builder.HasIndex(c => c.TenantId);
        builder.HasIndex(c => c.RoomId)
            .HasDatabaseName("ux_contracts_room_id_active")
            .IsUnique()
            .HasFilter("status = 'Active' AND is_deleted = false");
        builder.HasOne(c => c.Room)
            .WithMany(r => r.Contracts)
            .HasForeignKey(c => c.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(c => c.Tenant)
            .WithMany(u => u.ContractsAsTenant)
            .HasForeignKey(c => c.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}
