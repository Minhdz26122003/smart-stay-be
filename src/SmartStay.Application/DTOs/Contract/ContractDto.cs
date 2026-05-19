using System;
using SmartStay.Domain.Enums;

namespace SmartStay.Application.DTOs.Contract;

public class ContractDto
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public Guid TenantId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public string TenantName { get; set; } = string.Empty;
    public string TenantPhone { get; set; } = string.Empty;
    public decimal DepositAmount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ContractStatus Status { get; set; }
    public string? ScannedContractUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
