using System;
using SmartStay.Domain.Enums;

namespace SmartStay.Domain.Entities;

public class Contract : BaseEntity
{
    public Guid RoomId { get; set; }
    public Guid TenantId { get; set; }
    public decimal DepositAmount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ContractStatus Status { get; set; } = ContractStatus.Active;
    public string? ScannedContractUrl { get; set; }
}
