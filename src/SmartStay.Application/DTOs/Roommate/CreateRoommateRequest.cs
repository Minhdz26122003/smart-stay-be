using System;

namespace SmartStay.Application.DTOs.Roommate;

public class CreateRoommateRequest
{
    public Guid ContractId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? CccdPhotoUrl { get; set; } // Encrypted URL based on DB spec
}
