using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStay.Domain.Entities;

namespace SmartStay.Infrastructure.Persistence.Configurations;

public class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.DepositAmount).HasColumnType("numeric(18,2)");
        builder.Property(c => c.Status).HasConversion<string>();
        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}
