using System;
using System.Collections.Generic;
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

    public Room? Room { get; set; }
    public User? Tenant { get; set; }
    public ICollection<Roommate> Roommates { get; set; } = new List<Roommate>();
    public ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
