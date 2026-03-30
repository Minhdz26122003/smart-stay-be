using SmartStay.Domain.Enums;

namespace SmartStay.Application.DTOs.Contract;

public class UpdateContractRequest
{
    public ContractStatus Status { get; set; }
    public string? ScannedContractUrl { get; set; }
}
