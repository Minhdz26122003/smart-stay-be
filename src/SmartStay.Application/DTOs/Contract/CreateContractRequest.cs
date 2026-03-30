using System;

namespace SmartStay.Application.DTOs.Contract;

public class CreateContractRequest
{
    public Guid RoomId { get; set; }
    public Guid TenantId { get; set; }
    public decimal DepositAmount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? ScannedContractUrl { get; set; }
}
